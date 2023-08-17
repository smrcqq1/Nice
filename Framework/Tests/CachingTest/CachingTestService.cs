namespace CachingTest
{
    public class CachingTestService : ICachingTestAPI
    {
        /// <summary>
        /// 获取缓存数据,此接口只管返回实时数据，框架会自动为该接口做缓存处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<string> GetStudentInfo(Guid id)
        {
            return Task.FromResult(StudentInfo);
        }
        static string StudentInfo = "学生1";
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <returns></returns>
        public Task EditStudentInfo(EditStudentRequest request)
        {
            StudentInfo = request.Name;
            return Task.CompletedTask;
        }
    }
}