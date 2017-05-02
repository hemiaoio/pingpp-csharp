// <com.woyouqiu.Copyright>
// --------------------------------------------------------------
// <copyright>上海有求网络科技有限公司 2015</copyright>
// <Solution>pingpp</Solution>
// <Project>PingPlusPlus.AspNetCore.Demo</Project>
// <FileName>ChargeModel.cs</FileName>
// <CreateTime>2017-05-02 13:18</CreateTime>
// <Author>何苗</Author>
// <Email>hemiao@woyouqiu.com</Email>
// <log date="2017-05-02 13:18" version="00001">创建</log>
// --------------------------------------------------------------
// </com.woyouqiu.Copyright>
namespace PingPlusPlus.AspNetCore.Demo.Models {
    public class ChargeModel {
        public decimal Amount { get; set; }
        public string Channel { get; set; }
        public string Order_no { get; set; }
    }
}