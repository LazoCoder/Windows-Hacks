using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace WindowsHacks
{

    /// <summary>
    /// For comparing images and searches for smaller images within a larger one.
    /// </summary>
    public static class BasicVision
    {

        /// <summary>
        /// Compare two equal sized bitmaps pixel by pixel.
        /// </summary>
        /// <param name="bmp1">The first bitmap to compare.</param>
        /// <param name="bmp2">The second bitmap to compare.</param>
        /// <returns>Percentage of similarity where 100% mean identical.</returns>
        public static double CompareByPixels(Bitmap bmp1, Bitmap bmp2)
        {
            // Bitmaps should be of equal size.
            if (bmp1.Size != bmp2.Size) throw new Exception("Images are not of equal sizes.");

            // Preparation for the percentage calculation.
            double numerator = 0;
            double denominator = bmp1.Width * bmp1.Height;

            unsafe
            {
                BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadWrite, bmp1.PixelFormat);
                BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadWrite, bmp2.PixelFormat);

                int bytesPerPixel = Image.GetPixelFormatSize(bmp1.PixelFormat) / 8;
                int heightInPixels = bitmapData1.Height;
                int widthInBytes = bitmapData1.Width * bytesPerPixel;
                byte* PtrFirstPixel1 = (byte*)bitmapData1.Scan0;
                byte* PtrFirstPixel2 = (byte*)bitmapData2.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine1 = PtrFirstPixel1 + (y * bitmapData1.Stride);
                    byte* currentLine2 = PtrFirstPixel2 + (y * bitmapData2.Stride);

                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        int blue1 = currentLine1[x];
                        int green1 = currentLine1[x + 1];
                        int red1 = currentLine1[x + 2];

                        int blue2 = currentLine2[x];
                        int green2 = currentLine2[x + 1];
                        int red2 = currentLine2[x + 2];

                        if (blue1 == blue2 && green1 == green2 && red1 == red2) numerator++;
                    }
                });
                bmp1.UnlockBits(bitmapData1);
                bmp2.UnlockBits(bitmapData2);
            }
            return Math.Round((numerator / denominator) * 100);
        }

        /// <summary>
        /// Returns a bitmap that displays the difference between two equal sized images.
        /// </summary>
        /// <param name="bmp1">The first bitmap.</param>
        /// <param name="bmp2">The second bitmap.</param>
        /// <param name="printImage">If true, matching pixels are drawn.</param>
        /// <returns>A bitmap with the difference.</returns>
        public static Bitmap PrintDifference(Bitmap bmp1, Bitmap bmp2, bool printImage)
        {
            // Bitmaps should be of equal size. 
            if (bmp1.Size != bmp2.Size) throw new Exception("Images are not of equal sizes.");

            // Create a blank canvas that will be drawn on and returned.
            Bitmap output = new Bitmap(bmp1.Width, bmp1.Height, bmp1.PixelFormat);
            using (Graphics graph = Graphics.FromImage(output))
            {
                Rectangle ImageSize = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
                graph.FillRectangle(Brushes.White, ImageSize);
            }

            unsafe
            {
                // Lock the bitmaps into memory.
                BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadWrite, bmp1.PixelFormat);
                BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadWrite, bmp2.PixelFormat);
                BitmapData outputData = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.ReadWrite, output.PixelFormat);

                int bytesPerPixel = Bitmap.GetPixelFormatSize(bmp1.PixelFormat) / 8;
                int bytesPerPixel2 = Bitmap.GetPixelFormatSize(bmp2.PixelFormat) / 8;
                int bytesPerPixel3 = Bitmap.GetPixelFormatSize(output.PixelFormat) / 8;

                int heightInPixels = bitmapData1.Height;
                int widthInBytes = bitmapData1.Width * bytesPerPixel;

                byte* PtrFirstPixel1 = (byte*)bitmapData1.Scan0;
                byte* PtrFirstPixel2 = (byte*)bitmapData2.Scan0;
                byte* PtrFirstPixel3 = (byte*)outputData.Scan0;

                // For each row in both bitmaps.
                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine1 = PtrFirstPixel1 + (y * bitmapData1.Stride);
                    byte* currentLine2 = PtrFirstPixel2 + (y * bitmapData2.Stride);
                    byte* currentLine3 = PtrFirstPixel3 + (y * outputData.Stride);

                    // For each pixel in that row.
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {

                        // RGB values of the pixel in the first bitmap.
                        int bmp1Blue = currentLine1[x];
                        int bmp1Green = currentLine1[x + 1];
                        int bmp1Red = currentLine1[x + 2];

                        // RGB values of the pixel in the second bitmap.
                        int bmp2Blue = currentLine2[x];
                        int bmp2Green = currentLine2[x + 1];
                        int bmp2Red = currentLine2[x + 2];

                        // If both pixels match, draw that pixel in the third bitmap.
                        if (bmp1Blue == bmp2Blue && bmp1Green == bmp2Green && bmp1Red == bmp2Red)
                        {
                            if (printImage)
                            {
                                currentLine3[x] = (byte)bmp1Blue;
                                currentLine3[x + 1] = (byte)bmp1Green;
                                currentLine3[x + 2] = (byte)bmp1Red;
                            }
                            else
                            {
                                currentLine3[x] = (byte)0;
                                currentLine3[x + 1] = (byte)0;
                                currentLine3[x + 2] = (byte)0;
                            }
                        }
                        // If the pixels do not match, draw the specified color instead.
                        else
                        {
                            currentLine3[x] = 255;
                            currentLine3[x + 1] = 255;
                            currentLine3[x + 2] = 255;
                        }
                    }
                });
                bmp1.UnlockBits(bitmapData1);
                bmp2.UnlockBits(bitmapData2);
                output.UnlockBits(outputData);
            }
            return output;
        }

        /// <summary>
        /// Uses lockbits to find a bitmap within a larger bitmap and returns the location as a point.
        /// If not found, returns (0, 0).
        /// </summary>
        /// <param name="needle">Bitmap to be found.</param>
        /// <param name="haystack">Bitmap to search.</param>
        /// <returns>True if bitmap found.</returns>
        public static bool FindBitmap(Bitmap needle, Bitmap haystack, out Point location)
        {
            // Check if needle is smaller than haystack.
            if (needle.Height > haystack.Height || needle.Width > haystack.Width) throw new Exception("Needle cannot be larger than haystack.");
            if (needle == haystack)
            {
                location = new Point(0, 0);
                return true;
            }

            unsafe
            {
                BitmapData haystackData = haystack.LockBits(new Rectangle(0, 0, haystack.Width, haystack.Height), ImageLockMode.ReadWrite, haystack.PixelFormat);
                int bytesPerPixel_haystack = System.Drawing.Bitmap.GetPixelFormatSize(haystack.PixelFormat) / 8;
                int heightInPixels_haystack = haystackData.Height;
                int widthInBytes_haystack = haystackData.Width * bytesPerPixel_haystack;
                byte* ptrFirstPixel_haystack = (byte*)haystackData.Scan0;

                BitmapData needleData = needle.LockBits(new Rectangle(0, 0, needle.Width, needle.Height), ImageLockMode.ReadWrite, needle.PixelFormat);
                int bytesPerPixel_needle = System.Drawing.Bitmap.GetPixelFormatSize(needle.PixelFormat) / 8;
                int heightInPixels_needle = needleData.Height;
                int widthInBytes_needle = needleData.Width * bytesPerPixel_needle;
                byte* ptrFirstPixel_needle = (byte*)needleData.Scan0;

                // For each row in the haystack.
                for (int outerY = 0; outerY < heightInPixels_haystack - heightInPixels_needle + 1; outerY++)
                {
                    byte* currentLine = ptrFirstPixel_haystack + (outerY * haystackData.Stride);

                    // For each pixel in that row (of the haystack).
                    for (int outerX = 0; outerX < widthInBytes_haystack - widthInBytes_needle + (1 * bytesPerPixel_needle); outerX = outerX + bytesPerPixel_haystack)
                    {

                        // For each row in the needle.
                        for (int innerY = 0; innerY < heightInPixels_needle; innerY++)
                        {
                            byte* currentLine2 = ptrFirstPixel_needle + (innerY * needleData.Stride);

                            // For each pixel in that row (of the needle).
                            for (int innerX = 0; innerX < widthInBytes_needle; innerX = innerX + bytesPerPixel_needle)
                            {
                                currentLine = ptrFirstPixel_haystack + ((outerY + innerY) * haystackData.Stride);

                                // Get RGB colours of haystack at this point.
                                int hBlue = currentLine[outerX + innerX];
                                int hGreen = currentLine[outerX + innerX + 1];
                                int hRed = currentLine[outerX + innerX + 2];

                                // Get RGB colours of needle at this point.
                                int nBlue = currentLine2[innerX];
                                int nGreen = currentLine2[innerX + 1];
                                int nRed = currentLine2[innerX + 2];

                                // Check if rgb values are different.
                                if (hBlue != nBlue || hGreen != nGreen || hRed != nRed)
                                {
                                    goto notFound; // Pixels did not match.
                                }
                            }
                        }

                        haystack.UnlockBits(haystackData);
                        needle.UnlockBits(needleData);

                        //outerX has to be converted to a coordinate, outerY is fine as it is
                        location = new Point(outerX / bytesPerPixel_haystack, outerY);
                        return true;

                        notFound:
                        continue;
                    }
                }
                haystack.UnlockBits(haystackData);
                needle.UnlockBits(needleData);
            }
            location = Point.Empty;
            return false;
        }
    }
}
