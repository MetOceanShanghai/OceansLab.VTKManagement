using ASA.NetCDF4;
using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OceansLab.VTKManagement
{
    public class HycomExtent
    {
        public void GetData(Commons.Map.ExtentF extent,string outputPath)
        {
            string path = @"D:\Data\meteodata\netcdf\hycom_glb_912_ts3z_2018040500_000.nc";
            NcFile file = new NcFile(path, NcFileMode.read);
            int xNum = file.GetDim("lon").GetSize();
            int yNum = file.GetDim("lat").GetSize();
            int zNum = file.GetDim("depth").GetSize();
            float[] lonArr = new float[xNum];
            float[] latArr = new float[yNum];
            file.GetVar("lon").GetVar(lonArr);
            file.GetVar("lat").GetVar(latArr);
            int startX = 0;
            int startY = 0;
            int endX = 0;
            int endY = 0;
            for (int i = 0; i < lonArr.Length; i++)
            {
                if (lonArr[i] > extent.XMin)
                {
                    startX = i - 1;
                    break;
                }
            }
            for (int i = 0; i < lonArr.Length; i++)
            {
                if (lonArr[i] > extent.XMax)
                {
                    endX = i;
                    break;
                }
            }
            for (int i = 0; i < latArr.Length; i++)
            {
                if (latArr[i] > extent.YMin)
                {
                    startY = i - 1;
                    break;
                }
            }
            for (int i = 0; i < latArr.Length; i++)
            {
                if (latArr[i] > extent.YMax)
                {
                    endY = i;
                    break;
                }
            }
            int countx = endX - startX + 1;
            int county = endY - startY + 1;
            float[] value = new float[countx * county * zNum];
            file.GetVar("water_temp").GetVar(new int[] { 0, 0, startY, startX }, new int[] { 1, zNum, county, countx }, new int[] { 1, 1, 1, 1 }, value);
            int[] dims = new int[] { countx, county, zNum };
            vtkImageData simage = vtkImageData.New();
            simage.SetDimensions(dims[0], dims[1], dims[2]);

            IntPtr pPixel;
            float minVal = 0;
            float maxVal = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == -30000)
                {
                    value[i] = 0;
                    continue;
                }
                value[i] = value[i] * 0.001f + 20;
                if (minVal > value[i])
                {
                    minVal = value[i];
                }
                if (maxVal < value[i])
                {
                    maxVal = value[i];
                }

            }
            int count = 0;
            for (int z = 0; z < zNum; z++)
            {
                for (int j = 0; j < county; j++)
                {
                    for (int i = 0; i < countx; i++)
                    {
                        if (value[count] == -9999)
                        {
                            continue;
                        }
                        // float[] pixel = new float[] { color.R, color.G, color.B };
                        float[] pixel = new float[] { value[count], value[count], value[count] };
                        // float[] pixel = new float[] { 10, 10, 20 };
                        pPixel = simage.GetScalarPointer(i, j, zNum - z - 1);
                        System.Runtime.InteropServices.Marshal.Copy(pixel, 0, pPixel, 3);
                        count++;
                    }
                }
            }
            vtkXMLImageDataWriter writer = vtkXMLImageDataWriter.New();
            writer.SetFileName(outputPath);
            writer.SetInput(simage);
            writer.Write();
            writer.Update();
        }
    }
}
