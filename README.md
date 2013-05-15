# CrossUI - A Portable .NET Drawing API focused on Instant Test Feedback

Programming languages provide the ultimate flexibility for parameterizing user interfaces 
and graphical output in general. But they lack the instant feedback that markup languages 
and visual editors provide. This drawback is not set in stone. With some effort and tool support,
it is possible to visualize code-generated drawings in realtime as you type.

## How

	using CrossUI;

	[assembly: DrawingBackend(typeof(CrossUI.SharpDX.DrawingBackend))]

	namespace MyDrawings
	{
		public class RoundedRectangleTest
		{
			[BitmapDrawingTest(Width=80, Height=40)]
			public void RoundedRect(IDrawingContext context)
			{
				context.RoundedRect(0, 0, 80, 40, 8);
			}
		}
	}

![Rounded Rectangle](https://github.com/pragmatrix/CrossUI/raw/master/RoundedRectangle.png)

## Why

There are a number of reasons for CrossUI.

* Markup languages and APIs are not portable. Modern user interface APIs like .NET WPF, WinRT, iOS, Mac OS X, 
and Android are all different and make porting an application's user interface very time consuming. CrossUI could provide an API and testing frontend for all drawings and user interface components.
* Some of the user interface APIs lack the most basic features, like rendering UI controls into a background bitmap or composing geometries. 
CrossUI could provide implementations that use alternative APIs (like Direct2D for WinRT).
* For graphical editors, regression testing and visualizing different drawing states side by side is not a primary focus. CrossUI's testrunner can already be used to visualize different states of the same drawing and will soon support visual diffs.
* Like Test Driven Development puts your implementation code into shape, 
Test Driven Visualizations could do the same for user interfaces. 
All the major platforms' user interface APIs feel bloated and are a pain to work with. 
CrossUI's instant testing feedback could be basis for user interface APIs that are simple, light and extensible.

## What

The most import Projects in the CrossUI solutions are:

### Libraries you need

* CrossUI.Portable: The portable C# based drawing abstraction that is inspired by [processing.org](http://www.processing.org).
* CrossUI.TestRunner.Portable: The (internal) test backend which need to be referenced by your test code.

### Tools you run

* CrossUI.Runner.WPF: A WPF based graphical test runner that automatically reruns tests and presents them when DLLs are changed. Here is a [screenshot of the test runner](http://twitpic.com/a5n3ad) in action.
* CrossUI.Runner.WPF.x86: The WPF runner to test DLLs that require a 32 bit execution.

### Backends you choose from

* CrossUI.SharpDX.WinRT: CrossUI's [SharpDX](http://sharpdx.org/) based WinRT backend.
* CrossUI.SharpDX: The [SharpDX](http://sharpdx.org/) desktop backend.

## Getting Started

### Prerequisites

* Visual Studio 2010 Service Pack 1 or Visual Studio 2012 RC
* [Nuget Version 2](http://www.nuget.org)
* [Portable Library Tools 2 Beta from Microsoft](http://visualstudiogallery.msdn.microsoft.com/b0e0b5e9-e138-410b-ad10-00cb3caf4981/)
* recommended: [.NET Demon](http://www.red-gate.com/products/dotnet-development/dotnet-demon/). A Visual Studio extension that continuously rebuilds your projects while you type.

### Step By Step

* Download and extract the ZIP file or clone the repository.
* Open CrossUI.sln in Visual Studio
* Build the solution
* Start CrossUI.Runner.WPF
* Press "Add Test.."
* Select "CrossUI.Tests.DrawingContext/bin/Debug/CrossUI.Tests.DrawingContext.dll"

The WPF Testrunner should now show some drawings. These drawings are test results of the tests in CrossUI.Tests.DrawingContext.dll.

* For trying the instant feedback, open a source file in the project CrossUI.Tests.DrawingContext and 
change the code that produces the test results. If you got .NET Demon installed, changes should appear as you type.

### ... and how to draw to Bitmaps in your Code

* Reference CrossUI.Portable from your project.
* Reference one of the Backends (for example CrossUI.SharpDX).

... and somewhere in your code:

	using (var backend = new DrawingBackend())
	using (var surface = backend.CreateBitmapDrawingSurface(Width, Height))
	{
		using (var target = surface.BeginDraw())
		{
			// draw here against target
		}

		// Returns the alpha-premultiplied (ARGB) bitmap.
		var bitmap = surface.ExtractRawBitmap();
	}

## Roadmap

* Complete the drawing API
	* Add bitmap loading and drawing
* Port the Drawing Backend to more platforms
	* Android via MonoDroid
	* iOS via MonoTouch
	* Mac OS X via MonoMac
	* ? GDI via System.Drawing
	* ? [Cairo](http://cairographics.org/) via [Mono.Cairo](http://www.mono-project.com/Mono.Cairo)
	* ? XAML
	* ? SVG
	* ? [CrossGraphics](https://github.com/praeclarum/CrossGraphics)
* Implement visual diffs

## Future

* Create an API and test-runner for user interface components, animations and gesture recognizers which are instant testable. 

## Contributions

For non-trivial contributations, please use the [issue tracker](https://github.com/pragmatrix/CrossUI/issues) to align your changes with the roadmap.

## License

Copyright (c) 2012, Armin Sander
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

- Redistributions of source code must retain the above copyright notice, 
	  this list of conditions and the following disclaimer.
- Redistributions in binary form must reproduce the above copyright
	  notice, this list of conditions and the following disclaimer in the
	  documentation and/or other materials provided with the distribution.
- Neither the name of Armin Sander nor the
	  names of its contributors may be used to endorse or promote products
	  derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL ARMIN SANDER BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
