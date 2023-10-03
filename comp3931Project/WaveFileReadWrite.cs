using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio;
using NAudio.Wave;

namespace comp3931Project
{
    public class WaveFileReadWrite
    {
        public static byte[] readFile(String filePath)
        {

            /*            WaveChannel32 volumeStream = new WaveChannel32(fileReader);

                        WaveOutEvent player = new WaveOutEvent();

                        player.Init(volumeStream);

                        player.Play();*/

            using (WaveFileReader fileReader = new WaveFileReader(filePath))
            {
                byte[] buffer = new byte[fileReader.Length];
                int read = fileReader.Read(buffer, 0, buffer.Length);

/*                double[] samples = new double[read / 2];
                for (int sampleIndex = 0; sampleIndex < read / 2; sampleIndex++)*/
/*                {
                    int sampleValue = BitConverter.ToInt16(buffer, sampleIndex * 2);
                    samples[sampleIndex] = sampleValue / 32768.0;
                }*/
                return buffer;
            }

            }

        public static void writeFile(byte[] arr, String filePath)
        {
            WaveFormat waveFormat = new WaveFormat(8000, 8, 2);
            using (WaveFileWriter writer = new WaveFileWriter(filePath, waveFormat))
            {
                writer.Write(arr, 0, arr.Length);
            };
        }
        }
    }

