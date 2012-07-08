using System;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using Toolbox;
using Device1 = SharpDX.Direct3D10.Device1;
using Factory = SharpDX.Direct2D1.Factory;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;
using MapFlags = SharpDX.Direct3D10.MapFlags;

namespace CrossUI.SharpDX.Drawing
{
	sealed class BitmapDrawingContext : IBitmapDrawingContext
	{
		readonly int _width;
		readonly int _height;

		readonly Device1 _device;
		readonly Texture2D _texture;
		readonly Factory _factory;

		public BitmapDrawingContext(int width, int height)
		{
			if (width < 0 || height < 0)
				throw new Exception("Negative BitmapDrawingContext's area");

			_width = width;
			_height = height;


			_device = new Device1(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);
			var textureDesc = new Texture2DDescription
			{
				MipLevels = 1,
				ArraySize = 1,
				BindFlags = BindFlags.RenderTarget,
				CpuAccessFlags = CpuAccessFlags.None,
				Format = Format.B8G8R8A8_UNorm,
				OptionFlags = ResourceOptionFlags.None,
				Width = _width,
				Height = _height,
				Usage = ResourceUsage.Default,
				SampleDescription = new SampleDescription(1, 0)
			};

			_texture = new Texture2D(_device, textureDesc);
			_factory = new Factory(FactoryType.SingleThreaded, DebugLevel.None);
		}

		public void Dispose()
		{
			_factory.Dispose();
			_texture.Dispose();
			_device.Dispose();
		}

		public IDisposable beginDraw(out IDrawingContext context)
		{
			var surface = _texture.AsSurface();

			var rtProperties = new RenderTargetProperties()
			{
				DpiX = 96,
				DpiY = 96,
				Type = RenderTargetType.Default,
				PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)
			};

			var renderTarget = new RenderTarget(_factory, surface, rtProperties);

			var c = new RenderTargetDrawingContext(renderTarget, _width, _height);
			context = c;

			renderTarget.BeginDraw();

			return new DisposeAction(() =>
				{
					renderTarget.EndDraw();

					c.Dispose();
					renderTarget.Dispose();
					surface.Dispose();
				});
		}

		public byte[] extractRawBitmap()
		{
			// use a cpu bound resource

			var textureDesc = new Texture2DDescription
			{
				MipLevels = 1,
				ArraySize = 1,
				BindFlags = BindFlags.None,
				CpuAccessFlags = CpuAccessFlags.Read,
				Format = Format.B8G8R8A8_UNorm,
				OptionFlags = ResourceOptionFlags.None,
				Width = _width,
				Height = _height,
				Usage = ResourceUsage.Staging,
				SampleDescription = new SampleDescription(1, 0)
			};

			using (var cpuTexture = new Texture2D(_device, textureDesc))
			{
				_device.CopyResource(_texture, cpuTexture);

				var bytesPerLine = _width * 4;
				var res = new byte[bytesPerLine * _height];
				var data = cpuTexture.Map(0, MapMode.Read, MapFlags.None);
				try
				{
					IntPtr sourcePtr = data.DataPointer;
					int targetOffset = 0;
					for (int i = 0; i != _height; ++i)
					{
						Marshal.Copy(sourcePtr, res, targetOffset, bytesPerLine);
						sourcePtr += data.Pitch;
						targetOffset += bytesPerLine;
					}

					return res;
				}
				finally
				{
					cpuTexture.Unmap(0);
				}
			}
		}
	}
}
