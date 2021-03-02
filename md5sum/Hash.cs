using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace md5sum
{
    class Hash
    {
        private const int bufSize = 1 << 20; /* 1MB缓存 */
        private static readonly byte[] tempBuf = new byte[bufSize];
        private static readonly StringBuilder text = new StringBuilder();

        private long nowBytes, allBytes;

        private readonly string sCase;
        private readonly int calcCount = 0;
        private readonly bool[] condition = { true, true, true, true, true, true, true };
        private readonly CancellationToken token;

        /* https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/lock-statement */
        /* private readonly object balanceLock = new object(); */

        public Hash(CancellationToken token, params bool[] vals)
        {
            this.token = token;

            for (int i = 0; i < condition.Length; i++)
            {
                condition[i] = vals[i];
                if (i >= 2 && i <= 5 && condition[i])
                    calcCount++; /* 多少种计算方式 */
            }

            if (condition[6])
                sCase = "0123456789ABCDEF";
            else
                sCase = "0123456789abcdef";
        }

        public delegate void UpdatePbUI(bool isNow, int val, int max);
        public UpdatePbUI UpdatePbDelegate; /* 更新进度条委托 */

        public delegate void AppendUIText(string text);
        public AppendUIText AppendTbShowText; /* 追加文本委托 */

        public delegate void StartStopUI(bool isStart);
        public StartStopUI UpdateStartStopUI; /* 启动结束任务更新UI属性 */

        public void Calculate(string[] paths)
        {
            UpdatePbDelegate(false, 0, paths.Length);
            UpdatePbDelegate(true, 0, 100); /* 初始化2个进度条 */
            UpdateStartStopUI(true); /* 启动任务,修改UI控件属性 */

            int count = 1;
            FileStream fr = null;
            foreach (string path in paths)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(path);

                    text.Clear().Append("文件路径：").AppendLine(fileInfo.FullName);
                    nowBytes = fileInfo.Length;
                    text.Append("文件大小：");
                    ConvertSizeToByte(nowBytes);

                    if (condition[0])
                    {
                        string version = System.Diagnostics.FileVersionInfo.GetVersionInfo(path).FileVersion;
                        if (!string.IsNullOrEmpty(version))
                            text.Append("文件版本：").AppendLine(version);
                    }
                    if (condition[1])
                        text.Append("修改时间：").AppendLine(fileInfo.LastWriteTime.ToString("F"));

                    AppendTbShowText(text.ToString()); /* 提前将基本信息显示 */
                    if (calcCount > 0)
                    {   /* 当勾选计算Hash时才进行计算 */
                        allBytes = calcCount * nowBytes;
                        nowBytes = 0;

                        fr = File.OpenRead(path);
                        if (condition[2])
                            CalculateHash(MD5.Create(), fr, "MD5    ：");
                        if (condition[3])
                            CalculateHash(SHA1.Create(), fr, "SHA1   ：");
                        if (condition[4])
                            CalculateHash(SHA256.Create(), fr, "SHA256 ：");
                        if (condition[5])
                            CalculateHash(Crc32.Create(), fr, "CRC32  ：");
                    }
                }
                catch (CancelException ex)
                {
                    AppendTbShowText(ex.Message + Environment.NewLine);
                    break; /* 会在执行完finally后退出for */
                }
                catch (Exception ex)
                {
                    AppendTbShowText(ex.Message + Environment.NewLine);
                }
                finally
                {
                    if (fr != null)
                        fr.Dispose();
                    AppendTbShowText(Environment.NewLine);
                    UpdatePbDelegate(false, count++, -1);
                }
            }
            UpdateStartStopUI(false); /* 结束任务,修改UI控件属性 */
        }

        private void CalculateHash(HashAlgorithm mi, FileStream fr, string prefix)
        {
            try
            {
                int num;
                fr.Seek(0, SeekOrigin.Begin); /* 定位到文件开头,计算hash */
                while ((num = fr.Read(tempBuf, 0, bufSize)) > 0)
                {
                    if (token.IsCancellationRequested)
                        throw new CancelException("操作取消");

                    mi.TransformBlock(tempBuf, 0, num, tempBuf, 0);

                    nowBytes += num;
                    /* 更新进度条,下面计算有点危险,但是一般也遇不到非常大的文件 */
                    UpdatePbDelegate(true, (int)(100 * nowBytes / allBytes), -1);
                }
                mi.TransformFinalBlock(tempBuf, 0, 0);

                text.Clear().Append(prefix); /* 写入前缀,将字节转换为16进制字符串 */
                foreach (byte b in mi.Hash)
                    text.Append(sCase[b >> 4]).Append(sCase[b & 0xf]);
                AppendTbShowText(text.AppendLine().ToString());
            }
            catch (CancelException)
            {
                throw; /* 交给上层处理,同时finally也会被执行 */
            }
            catch (Exception ex)
            {
                AppendTbShowText(ex.Message + Environment.NewLine);
            }
            finally
            {
                mi.Dispose();
            }
        }

        /// <summary>
        /// 根据size计算文件对应单位的值
        /// </summary>
        private static readonly long[] DivSize = { (long)1 << 10, (long)1 << 20, (long)1 << 30, (long)1 << 40, (long)1 << 50 };
        private static readonly string[] ByteUnit = { " B", " KB", " MB", " GB", " TB" };
        private static void ConvertSizeToByte(long size)
        {
            int i = 0;
            for (; i < ByteUnit.Length; i++)
                if (size < DivSize[i])
                    break;
            text.Append(size).Append(ByteUnit[0]);
            if (i > 0 && i < ByteUnit.Length)
                text.Append("(").Append(((double)size / DivSize[i - 1]).ToString("f2")).Append(ByteUnit[i]).AppendLine(")");
            else
                text.AppendLine();
        }
    }

    sealed class CancelException : Exception
    {
        public CancelException(string message) : base(message) { }
    }

    /// <summary>
    /// 计算crc32的源码
    /// https://github.com/damieng/DamienGKit/blob/master/CSharp/DamienG.Library/Security/Cryptography/Crc32.cs
    /// </summary>
    sealed class Crc32 : HashAlgorithm
    {
        public const UInt32 DefaultPolynomial = 0xedb88320u;
        public const UInt32 DefaultSeed = 0xffffffffu;

        static UInt32[] defaultTable;

        readonly UInt32 seed;
        readonly UInt32[] table;
        UInt32 hash;

        public new static Crc32 Create()
        {
            return new Crc32(DefaultPolynomial, DefaultSeed);
        }

        public Crc32(UInt32 polynomial, UInt32 seed)
        {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("Not supported on Big Endian processors");

            table = InitializeTable(polynomial);
            this.seed = hash = seed;
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            hash = CalculateHash(table, hash, array, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize { get { return 32; } }

        public static UInt32 Compute(byte[] buffer)
        {
            return Compute(DefaultSeed, buffer);
        }

        public static UInt32 Compute(UInt32 seed, byte[] buffer)
        {
            return Compute(DefaultPolynomial, seed, buffer);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        static UInt32[] InitializeTable(UInt32 polynomial)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null)
                return defaultTable;

            UInt32[] createTable = new UInt32[256];
            for (int i = 0; i < 256; i++)
            {
                UInt32 entry = (UInt32)i;
                for (int j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry >>= 1;
                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial)
                defaultTable = createTable;

            return createTable;
        }

        static UInt32 CalculateHash(UInt32[] table, UInt32 seed, IList<byte> buffer, int start, int size)
        {
            UInt32 hash = seed;
            for (int i = start; i < start + size; i++)
                hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
            return hash;
        }

        static byte[] UInt32ToBigEndianBytes(UInt32 uint32)
        {
            byte[] result = BitConverter.GetBytes(uint32);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);
            return result;
        }
    }
}
