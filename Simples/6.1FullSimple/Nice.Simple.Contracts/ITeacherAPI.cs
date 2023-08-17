namespace Nice.Simple.Contracts;
/// <summary>
/// 教师管理服务
/// </summary>
/// <remarks>
/// 
/// </remarks>
public interface ITeacherAPI : IAPI
{
    /// <summary>
    /// 获取教师列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    Task<List<DTO.Teacher>> ListAsync();
    /// <summary>
    /// 获取教师列表
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    Task<Guid> AddAsync();
    /// <summary>
    /// 获取教师列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    Task<TeacherStatus?> CheckStatusAsync(Guid id);
}