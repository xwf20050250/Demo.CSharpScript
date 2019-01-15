using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Demo.CSharpScript.Models;
using Demo.CSharpScript.Helpers;
using System.Text;
using Microsoft.CodeAnalysis.Scripting;
using CS = Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Demo.CSharpScript.Controllers
{
    public class DemoController : Controller
    {
        const string SCRIPT_FILE_NAME_Before = "Before.script";
        const string SCRIPT_FILE_NAME_After = "After.script";

        /// <summary>
        /// 配置脚本的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult ConfigView()
        {
            ViewData["Before"] = GetOrSetScript(SCRIPT_FILE_NAME_Before);
            ViewData["After"] = GetOrSetScript(SCRIPT_FILE_NAME_After);
            return View();
        }

        /// <summary>
        /// 保存脚本
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        public IActionResult ConfigSave(string before, string after)
        {
            ViewData["Before"] = GetOrSetScript(SCRIPT_FILE_NAME_Before, before);
            ViewData["After"] = GetOrSetScript(SCRIPT_FILE_NAME_After, after);
            return View("ConfigView");
        }

        public IActionResult GetData(string name)
        {
            try
            {
                //测试数据
                var data = DemoModel.GetDemoDatas();

                StringBuilder builder = new StringBuilder();
                builder.Append("public class DataDeal");
                builder.Append("{");
                builder.Append(GetOrSetScript(SCRIPT_FILE_NAME_Before));
                builder.Append("}");

                var script = CS.CSharpScript.Create<string>(builder.ToString()
                    , globalsType: typeof(Arg));

                script.Compile();

                script.ContinueWith<string>("new DataDeal().Before(Name);", ScriptOptions.Default);

                script.Compile();

                var ss = script.RunAsync(new Arg { Name = name }).Result.ReturnValue;

                if (!string.IsNullOrEmpty(ss))
                {
                    data = data.Where(t => t.Name.Contains(ss)).ToList();
                }

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json($"出错了,错误日志：{ex.ToString()}");
            }
        }


        /// <summary>
        /// 获取或设置脚本内容
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private string GetOrSetScript(string file, string script = null)
        {
            //修改
            if (!string.IsNullOrEmpty(script))
            {
                if (!System.IO.File.Exists(file))
                    System.IO.File.Create(file);

                //false means overwrite the file
                using (StreamWriter writer = new StreamWriter(file, false))
                {
                    writer.AutoFlush = true;
                    writer.WriteLine(script);
                }

                return script;
            }

            //查询
            if (System.IO.File.Exists(file))
                return System.IO.File.ReadAllText(file);

            return null;
        }
    }
}