using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Controllers;
using WebApi.Models.Repositories;

namespace XUnitTestProject.Controller
{
    public class OrderControllerTest
    {
        private readonly Mock<IUsersRepository> _mockUserRepo;
        private readonly Mock<IDriverRepository> _mockDriverRepo;
        private readonly Mock<IOrderRepository> _mockOrderRepo;
        private readonly OrderController _sut;
        public OrderControllerTest()
        {
            //_mockRepository = new Mock<IOrderRepository>();
            //_sut = new OrderController(_mockOrderRepo, _mockUserRepo;
        }
    }
}
