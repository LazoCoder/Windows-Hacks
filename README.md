# Windows-Hacks
Creative and unusual things that can be done with the Windows API.

## Shrinking Windows
Any window can be shrunk down.

Shrinking the Calculator:

![alt-tag](gifs/shrinking_calc.gif)

Shrinking Google Chrome:

![alt-tag](gifs/shrinking_chrome.gif)

Even Microsoft Word is not safe from being shrunk:

![alt-tag](gifs/shrinking_ms_word.gif)

## Hue Shifter

Any window can be selected to have its colors shifted. 

Hue shifting Microsoft Paint:

![alt-tag](gifs/hue_shifting_ms_paint.gif)

Hue shifting Microsoft Word:

![alt-tag](gifs/hue_shifting_word.gif)

## Writing Directly To The Desktop

![alt-tag](gifs/writing_to_desktop.png)

## Desktop Rotation

Zooms into the Desktop and then spins it around and around.

![alt-tag](gifs/desktop_rotation.gif)

## Window Shaker

Randomly shakes the specified window without the use of a mouse.

Here you can see Microsoft Paint shaking all by itself. Pretty spooky huh?

![alt-tag](gifs/shaking_microsoft_paint.gif)

If you are a masterful artist (such as myself), you may find that it is too easy to draw in a stationary canvas. In that case, this is the perfect utility for you.

![alt-tag](gifs/drawing_in_a_shaky_microsoft_paint.gif)

## Wavy Window

![alt-tag](gifs/wave_scream.gif)

## Motion Detection

Detects motion in the specified window. This was a lot of fun to play with, especially when used on videos but I can only include so many gifs...

Detecting motion in Microsoft Paint:

![alt-tag](gifs/motion_detection_paint.gif)

Detecting motion in command prompt:

![alt-tag](gifs/motion_detection_cmd.gif)

## Automated Painting

Automatically draws a thresholded Mona Lisa into Microsoft Paint:

![alt-tag](gifs/mona_lisa.gif)

## Window Scrambler

![alt-tag](gifs/scrambling_a_window.gif)

## Window Thresholding

This makes the light colors transparent and the dark colors black.

Before thresholding:

![alt-tag](gifs/thresholding_before.png)

After thresholding:

![alt-tag](gifs/thresholding_after.png)

## Force Changing Window Properties

Many of the properties of a Window can be force changed. In the image below I have changed the following properties in the Windows Calculator:
 - Disabled the close, maximimize and minimize buttons
 - Removed the drop down menu (View, Edit, Help)
 - Changed the width and height of the Window borders
 - Changed the title
 
![alt-tag](gifs/calculator_changed.png)
 
## Mouse Spam

Spawns hundreds of mice, where each one moves independently across the screen.

![alt-tag](gifs/mouse_spam.png)

## Hiding a Window

Any Window can be hidden. In the following image I have hidden the Windows Calculator. As you can see, it is no where to be found on the Desktop. But if you open the Windows Task Manager it is actually still running in the background:

![alt-tag](gifs/hidden_window.png)

## More...

Here is the menu. It lists all of the features:

![alt-tag](gifs/menu.png)

## How to Run

The compiled binary is located in the folder *Executable*.

If you want to compile it yourself the recommended way is as follows:
1. Make sure you have Visual Studio and .NET Framework 4.5.2.
2. Download the zip.
3. Unzip it.
4. Open WindowsHacks.sln in Visual Studio.
5. Press F5 to run it.

## Notes

 - The main class that brings all of the functions together is *Program.cs*.

 - The to do list can be found in the top directory. This includes bugs to fix and new features to implement.

 - The External folder contains two essential .dll files that must be referenced for the code to work. These two files are WindowsAPI.dll and and ImageProcessing.dll and the code for these can be seen in my [WinAPI-Wrapper](https://github.com/LazoCoder/WinAPI-Wrapper) and [Image-Processing-Library](https://github.com/LazoCoder/Image-Processing-Library) repositories respectively. 

 - This has only been tested on Windows 7.
