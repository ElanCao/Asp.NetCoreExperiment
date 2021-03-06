﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Language;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GraphQLDemo01.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ISchema _schema;
        public TestController(ISchema schema, ILogger<TestController> logger)
        {
            _schema = schema;
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get(string query)
        {
            var executor = _schema.MakeExecutable();
            var executionResult = await executor.ExecuteAsync(query);
            return Regex.Unescape(executionResult.ToJson()); ;
        }
    }
    public class Query
    {
        private readonly List<Item> _items;
        public Query()
        {
            _items = new List<Item>() {
                new Item { Id=1,Name="一级A",Istrue=true },
                new Item { Id=2,Name="一级B",Istrue=false },
                new Item { Id=3,Name="一级C",Istrue=true },
                new Item { Id=4,Name="一级A-二级A",Pid=1,Istrue=false  },
                new Item { Id=5,Name="一级A-一级B",Pid=1,Istrue=true },
                new Item { Id=6,Name="一级B-二级A",Pid=2 ,Istrue=false }
            };
        }

        public Item GetItem(int id)
        {
            return _items.SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Item> Queryistrue(bool istrue)
        {
            return _items.Where(s => s.Istrue == istrue);
        }
    
        public IEnumerable<Item> GetItems()
        {
            return _items;
        }
        public IEnumerable<Item> GetEntity()
        {
            return _items;
        }
        public IEnumerable<Item> GetChilds(int pid)
        {
            return _items.Where(s => s.Pid == pid);
        }


    }
    /// <summary>
    /// 实体类
    /// </summary>
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Pid { get; set; }
        public bool Istrue { get; set; }
    }
}
