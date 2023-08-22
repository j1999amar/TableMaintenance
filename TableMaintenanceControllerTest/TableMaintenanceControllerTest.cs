using AOTableDTOModel;
using AOTableInterface;
using AOTableModel;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Mapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableMaintenance.Controllers;
using Xunit;

namespace TableMaintenanceTest
{
    public class TableMaintenanceControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IAOTableInterface> _tableMock;
        private readonly IMapper _mapper;
        private readonly TableMaintenanceController _sut;

        public TableMaintenanceControllerTest()
        {
            _fixture = new Fixture();
            _tableMock = _fixture.Freeze<Mock<IAOTableInterface>>();
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _mapper = mapConfig.CreateMapper();
            _sut = new TableMaintenanceController(_tableMock.Object, _mapper);
        }

        [Fact]
        public async void SearchTable_ShouldReturnOkResponse_WhenAOTableHasTableList()
        {
            //Arrange
            var tableName=_fixture.Create<string>();
            var typeList = _fixture.Create<string[]>();
            var expectedTableList=_fixture.Create<ICollection<AOTable>>();
             _tableMock.Setup(x => x.GetTable(tableName, typeList)).ReturnsAsync(expectedTableList);

            //Act
            var result = await _sut.SearchTable(tableName, typeList);

            //Assertion
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<AOTableDTO>>>();
            result.Result.Should().BeOfType<OkObjectResult>();
            _tableMock.Verify(x=>x.GetTable(tableName, typeList),Times.Once);

        }

        [Fact]
        public async void SearchTable_ShouldReturnNotFoundResponse_WhenAOTableHasZeroData()
        {
            //Arrange
            var tableName = _fixture.Create<string>();
            var typeList = _fixture.Create<string[]>();
            ICollection<AOTable> expectedTableList = null;
            _tableMock.Setup(x => x.GetTable(tableName, typeList)).ReturnsAsync(expectedTableList);

            //Act
            var result = await _sut.SearchTable(tableName, typeList);

            //Assertion
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<AOTableDTO>>>();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            _tableMock.Verify(x => x.GetTable(tableName, typeList), Times.Once);



        }

    }

    
}
