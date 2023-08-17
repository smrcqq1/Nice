using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using System.Text;

namespace Nice.WebAPI
{
    public interface IFileAPI : Nice.IAPI
    {
        Task<string[]> Upload();
    }
    public class FileService : IFileAPI
    {
        public FileService(IHttpContextAccessor httpContext,IHostingEnvironment hostingEnvironment)
        {
            HttpContext = httpContext.HttpContext;
            this.hostingEnvironment = hostingEnvironment;
        }
        HttpContext HttpContext;
        IHostingEnvironment hostingEnvironment;
        public async Task<string[]> Upload()
        {
            //获取上传文件
            IFormFileCollection files = HttpContext.Request.Form.Files;
            //判断是否有文件上传
            if (files.Count == 0)
            {
                throw new Nice.BuzinessException("没有发现上传的文件");
            }
            List<string> md5s = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                //将流写入文件
                using Stream stream = files[i].OpenReadStream();
                // 把 Stream 转换成 byte[]
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);

                var md5 = GetMD5HashFromFile(stream);
                md5s.Add(md5);
                //设置文件上传路径
                string fileHead = "FileUpload";
                string fullFileName = Path.Combine(hostingEnvironment.ContentRootPath, fileHead, md5);
                ////创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                #region 检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                #endregion
                //保存文件  文件存在则先删除原来的文件
                if (System.IO.File.Exists(fullFileName))
                {
                    System.IO.File.Delete(fullFileName);
                }
                // 把 byte[] 写入文件
                using FileStream fs = new (fullFileName, FileMode.Create);
                using BinaryWriter bw = new (fs);
                bw.Write(bytes);
                fs.Close();
                bw.Close();
                await fs.DisposeAsync();
                await bw.DisposeAsync();
                stream.Close();
                await stream.DisposeAsync();
            }
            GC.Collect();
            return md5s.ToArray();
        }/// <summary>
         /// 计算文件的MD5值
         /// </summary>
         /// <param name="fileName"></param>
         /// <returns></returns>
        public static string GetMD5HashFromFile(Stream file)
        {
            //MD5加密服务提供器
            MD5 md5 = new MD5CryptoServiceProvider();

            //对文件进行计算MD5
            byte[] retVal = md5.ComputeHash(file);

            //保存输出结果
            StringBuilder sb = new StringBuilder();
            //转为2进制
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}