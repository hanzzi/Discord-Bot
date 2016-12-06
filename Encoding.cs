public class EncodingConverter
        {
            public static string BinarytoString(string Text)
            {
                string Data = Regex.Replace(Text, @"\s+", "");
                List<Byte> List = new List<Byte>();

                for (int i = 0; i < Data.Length; i += 8)
                {
                    List.Add(Convert.ToByte(Data.Substring(i, 8), 2));
                }
                return Encoding.ASCII.GetString(List.ToArray());

            }
            
            
            public static string StringToBinary(string data)
            {
                string value = string.Empty;

                byte[] ByteText;
                ByteText = System.Text.Encoding.UTF8.GetBytes(data);
                Array.Reverse(ByteText);
                BitArray bit = new BitArray(ByteText);
                StringBuilder sb = new StringBuilder();

                for (int i = bit.Length - 1; i >= 0; i--)
                {
                    if (bit[i] == true)
                    {
                        sb.Append(1);
                    } else
                    {
                        sb.Append(0);
                    }
                }
                return sb.ToString();

            }

            public static string StringToHex(string Data)
            {
                byte[] bytearray = Encoding.Default.GetBytes(Data);
                string HexString = BitConverter.ToString(bytearray);
                HexString = HexString.Replace("-", "");
                return HexString;
            }

            public static string HexToString(string Data)
            {
                if (Data == null)
                {
                    throw new ArgumentNullException("Empty string");
                }
                if (Data.Length % 2 != 0)
                {
                    throw new ArgumentException("String must have an even length");
                }
                byte[] bytes = new byte[Data.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    string CurrentHex = Data.Substring(i * 2, 2);
                    bytes[i] = Convert.ToByte(CurrentHex, 16);
                }
                string ReturnValue = Encoding.GetEncoding("UTF-8").GetString(bytes);
                return ReturnValue.ToString();
            }
        }
