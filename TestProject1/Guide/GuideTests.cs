//using MasterService;
//using MasterStructureAPIs.Data;
//using MasterStructureAPIs.Share.Services.Guide;
//using MasterStructureAPIs.Share.ShareRepositories;//repos
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Models.Share.Guide;
//using Moq;
////using System.Text.Json;
//using Oracle.ManagedDataAccess.Client;

namespace Guide
{
    public class GuideTests
    {
        //private readonly Mock<IMasterService> _repo;//_repos
        //private readonly Mock<IDBManager> _dbm;
        //private readonly GuideService service;
        //OracleConnection con;
        //OracleTransaction trn;

        //public GuideTests()
        //{
        //    var options = new DbContextOptionsBuilder<DataContext>()
        //                    .UseOracle(Globals.getConfigOracle())
        //                    .Options;
        //    var mockContext = new Mock<DataContext>(options);
        //    var mockConfiguration = new Mock<IConfiguration>();
        //    var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //    var mockLogger = new Mock<ILogger<GuideService>>();

        //    _dbm = new Mock<IDBManager>();
        //    _dbm.Setup(x => x.GetConnection("")).Returns(new OracleConnection());
        //    _repo = new Mock<IShareRepository>();
        //    service = new GuideService(mockContext.Object, mockConfiguration.Object, mockHttpContextAccessor.Object, mockLogger.Object, _dbm.Object, _repo.Object);
        //}

        //[Theory]
        //[Trait("PutBackGroundLog", "Call Function")]
        //[InlineData("300310", "TestOK", "chokpaisan.sri", null)]
        //[InlineData("300310", "TestOK", null, "PO-EST")]
        //[InlineData("300310", null, "chokpaisan.sri", null)]
        //[InlineData("300310", "TestOK", "chokpaisan.sri", "PO-EST")]
        //public void InsertBackGroundLog_SucessCase_ReturnBool(string PlantCode, string Message, string UserId, string ProgramCode)
        //{
        //    // Arrange
        //    OracleConnection testConnection = new OracleConnection();
        //    var req = new ReqInsertBackGroundLog
        //    {
        //        PlantCode = PlantCode,
        //        Message = Message,
        //        UserId = UserId,
        //        ProgramCode = ProgramCode
        //    };

        //    var expectedResult = true; // Set the expected result to true or false, depending on your expected behavior.

        //    _dbm.Setup(cs => cs.GetConnection(PlantCode)).Returns(testConnection);
        //    _dbm.Setup(cs => cs.CloseConnection(testConnection)).Verifiable();
        //    _repo.Setup(ir => ir.PutBackGroundLog(testConnection, req)).Returns(expectedResult);

        //    // Act
        //    var result = service.PutBackGroundLog(req);

        //    // Assert
        //    Assert.Equal(expectedResult, result.Data);

        //    // You may need to add verification for other expected behaviors, such as Commit and Rollback.
        //    _dbm.Verify(exp => exp.Commit(trn), Times.Once);
        //    _dbm.Verify(exp => exp.Rollback(trn), Times.Never);

        //}

        //[Fact]
        //[Trait("PutBackGroundLog", "Throws")]
        //public void InsertBackGroundLog_ThrowException()
        //{
        //    // Arrange
        //    OracleConnection testConnection = new OracleConnection();

        //    var req = new ReqInsertBackGroundLog
        //    {
        //        PlantCode = "",
        //        Message = "THA",
        //        UserId = "chokpaisan.sri",
        //        ProgramCode = ""
        //    };

            //Act
            //Assert.Throws<Exception>(() => service.PutBackGroundLog(req));

            // Assert
            //_dbm.Verify(cs => cs.CloseConnection(testConnection), Times.Once);
            //_repo.Verify(ir => ir.GetFormLocationByLotListDL(testConnection, req), Times.Once);
        //}
    }
}
