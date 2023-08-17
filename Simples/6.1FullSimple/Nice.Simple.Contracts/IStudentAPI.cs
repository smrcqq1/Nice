using Nice.Simple.Contracts.DTO;
using Nice.Simple.Contracts.DTO.Students;

namespace Nice.Simple.Contracts;
/// <summary>
/// 学生管理
/// </summary>
/// <remarks>
/// 打上接口标记以后可获得:
/// 1.Service层需要实现此接口
/// 2.不需要再写controller层，IL会根据通信协议等因素，自动生成对应的宿主API
/// </remarks>
//todo  【一般】自动生成WebSocket接口层未实现
//todo  【可选】自动生成Socket接口层未实现
//todo  【可选】自动生成Udp接口层未实现
public interface IStudentAPI : IAPI
{
    [Authorize]
    /// <summary>
    /// 新增学生
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// 1.使用各种Attributes来对接口进行注解，框架会自动生成对应的代码
    /// 2.如果不使用RouteAttribute则自动映射为名称去掉Async,如本接口自动映射为Student/Add
    /// 3.标注的BuzinessError为该接口可能发生的业务逻辑上的错误，需要业务逻辑代码中手动中处理。这里的业务逻辑错误仅指与数据库数据有关的错误，而不包括参数检查的错误，参数检查是框架自动进行的，无须手写代码也无需单元测试。
    /// 4.HttpMethod可以不指定，默认为POST，个人比较喜欢全部用POST，这样客户端调用不需要考虑这些无关紧要的事情，封装起来也更简单
    /// </remarks>
    [BuzinessError("不能为已离职教师分配学生")]
    [BuzinessError("学生姓名不能重复")]
    Task<Guid> AddAsync(AddStudentRequest request);
    /// <summary>
    /// 学生管理页面的初始化数据聚合
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// 
    /// </remarks>
    Task<AggrResult> AggrAsync(AggrRequest request);
    /// <summary>
    /// 删除指定ID的学生
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>
    /// 
    /// </remarks>
    Task DelAsync(Guid id);
    /// <summary>
    /// 分页查询符合条件的学生
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// 
    /// </remarks>
    Task<PageResult<StudentItem>> PageAsync(PageRequest request);
    /// <summary>
    /// 查询指定ID学生的详细信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>
    /// 这里默认采用POST方式，并且参数是值类型，也可以成功调用
    /// </remarks>
    Task<StudentInfo> InfoAsync(Guid id);
    /// <summary>
    /// 修改指定ID学生的详细信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>
    /// 
    /// </remarks>
    [BuzinessError("不能为已离职教师分配学生")]
    [BuzinessError("学生姓名不能重复")]
    [Route("Edit")]
    Task PostAsync(EditStudentRequest request);
}