# CrossUI - Portable Drawing API focused on Real-Time Test Feedback

Programming languages provide the ultimate flexibility for parameterizing user interfaces 
and graphical output in general. But they lack the real-time feedback that markup languages 
and visual editors provide. This drawback is not set in stone. With some effort and tool support,
it is possible to visualize code-generated drawings in realtime as you type.

## How

	using CrossUI;

	namespace MyDrawings
	{
		public class RoundedRectangleTest
		{
			[BitmapDrawingTest(Width=80, Height=40)]
			public void roundedRect(IDrawingContext context)
			{
				context.roundedRect(0, 0, 80, 40, 8);
			}
		}
	}

![Rounded Rectangle](https://github.com/pragmatrix/CrossUI/raw/master/RoundedRectangle.png)

## Why

There are a number of reasons for CrossUI.

* Markup languages and APIs are not portable. Modern user interface APIs like .NET WPF, WinRT, iOS, Mac OS X, 
and Android are completely different and make porting an application's user interface very time consuming. CrossUI could provide an API and testing frontend for all drawings and user interface components.
* Some of the user interface APIs lack the most basic features, like rendering UI controls into a background bitmap or composing geometries. 
CrossUI could provide implementations that use alternative APIs (like Direct2D for WinRT).
* For graphical editors, regression testing and visualizing different drawing states side by side is not a primary focus. CrossUI's testrunner can already be used to visualize different states of the same drawing and will soon support visual diffs.
* Like Test Driven Development puts your implementation code into shape. 
Test Driven Visualizations could do the same for user interfaces. 
All the major platform's user interface APIs feel bloated and are a pain to work with. 
CrossUI's real-time testing feedback could be basis for user interface APIs that are simple, light and extensible.

## What

Right now, CrossUI consists of 

* CrossUI.Portable: a portable C# based drawing abstraction that is inspired by [processing.org](http://www.processing.org)
* CrossUI.SharpDX: a SharpDX based Direct2D rendering implementation.
* CrussUI.Runner.WPF: A WPF based testrunner that automatically reruns tests when DLLs are 

## Getting Started

### Prerequisites

* Visual Studio 2010 Service Pack 1 or Visual Studio 2012
* [Nuget Version 2](http://www.nuget.org)
* [Portable Library Tools 2 Beta from Microsoft](http://visualstudiogallery.msdn.microsoft.com/b0e0b5e9-e138-410b-ad10-00cb3caf4981/)
* recommended: [.NET Demon](http://www.red-gate.com/products/dotnet-development/dotnet-demon/). A Visual Studio extension that continuously rebuilds your projects while you type.

### Step By Step

* Download and extract the ZIP file or clone the repository.
* Open CrossUI.sln in Visual Studio
* Build the solution
* Start CrossUI.Runner.WPF
* Press "Add Test.."
* Select "CrossUI.Tests/bin/CrossUI.Tests.dll"

The WPF Testrunner should now show some drawings. These drawings are test results of the tests in CrossUI.Tests.dll.

* Open CrossUI.Tests/TestRunnerTests.cs

In here you can change the code that produce the test results. If you got .NET Demon installed, changes should appear as you type.

## Roadmap

* Complete the drawing API
	* Implement paths
* Port the Drawing Backend to more platforms, preferable iOS via MonoTouch and Android via MonoDroid.
* Implement visual diffs

## Future

* Create an API and test-runner for user interface components, animations and gesture recognizers which are real-time-testable. 

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
- Neither the name of Armin Snader nor the
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
