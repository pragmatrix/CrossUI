using System;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using CrossUI.Toolbox;
using CrossUI.Drawing;
using Device1 = SharpDX.Direct3D10.Device1;
using Factory = SharpDX.Direct2D1.Factory;
using MapFlags = SharpDX.Direct3D10.MapFlags;

namespace CrossUI.SharpDX.Drawing
{
	sealed class BitmapDrawingTarget : IBitmapDrawingTarget
	{
		readonly DrawingBackend _backend;
		readonly Factory _factory;
		readonly Device1 _device;

		readonly int _width;
		readonly int _height;

		readonly Texture2D _texture;

		public BitmapDrawingTarget(DrawingBackend backend, int width, int height)
		{
			if (width < 0 || height < 0)
				throw new Exception("Area of BitmapDrawingTarget's is neagative");

			_factory = backend.Factory;
			_device = backend.Device;

			_backend = backend;
			_width = width;
			_height = height;

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
		}

		public void Dispose()
		{
			_texture.Dispose();
		}

		public IDisposable BeginDraw(out IDrawingTarget target)
		{
			var surface = _texture.AsSurface();

			var rtProperties = new RenderTargetProperties
			{
				DpiX = 96,
				DpiY = 96,
				Type = RenderTargetType.Default,
				PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)
			};

			var renderTarget = new RenderTarget(_factory, surface, rtProperties);
			var state = new DrawingState();
			var transform = new DrawingTransform();

			var drawingTargetImplementation = new DrawingTarget(state, transform, renderTarget, _width, _height);

			target = new DrawingTargetSplitter(
				_backend, 
				state, 
				transform, 
				drawingTargetImplementation, 
				drawingTargetImplementation, 
				drawingTargetImplementation, 
				drawingTargetImplementation, 
				drawingTargetImplementation);

			renderTarget.BeginDraw();
			// required for some graphic cards
			renderTarget.Clear(null);

			return new DisposeAction(() =>
				{
					renderTarget.EndDraw();

					drawingTargetImplementation.Dispose();
					renderTarget.Dispose();
					surface.Dispose();
				});
		}

		public byte[] ExtractRawBitmap()
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
