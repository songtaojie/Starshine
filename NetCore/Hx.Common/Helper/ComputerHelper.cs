// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;


namespace Hx.Common
{
    /// <summary>
    /// 计算机帮助类
    /// </summary>
    public static class ComputerHelper
    {
        /// <summary>
        /// 内存信息
        /// </summary>
        /// <returns></returns>
        public static MemoryMetrics GetComputerInfo()
        {
            MemoryMetricsClient client = new MemoryMetricsClient();
            MemoryMetrics memoryMetrics = IsUnix() ? client.GetUnixMetrics() : client.GetWindowsMetrics();

            memoryMetrics.FreeRam = Math.Round(memoryMetrics.Free / 1024, 2) + "GB";
            memoryMetrics.UsedRam = Math.Round(memoryMetrics.Used / 1024, 2) + "GB";
            memoryMetrics.TotalRam = Math.Round(memoryMetrics.Total / 1024, 2) + "GB";
            memoryMetrics.RamRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + "%";
            memoryMetrics.CpuRate = Math.Ceiling(double.Parse(GetCPURate())) + "%";
            return memoryMetrics;
        }

        /// <summary>
        /// 磁盘信息
        /// </summary>
        /// <returns></returns>
        public static List<DiskInfo> GetDiskInfos()
        {
            List<DiskInfo> diskInfos = new List<DiskInfo>();

            if (IsUnix())
            {
                string output = ShellUtil.Bash("df -m / | awk '{print $2,$3,$4,$5,$6}'");
                var arr = output.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 0) return diskInfos;

                var rootDisk = arr[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                if (rootDisk == null || rootDisk.Length == 0)
                    return diskInfos;

                DiskInfo diskInfo = new DiskInfo()
                {
                    DiskName = "/",
                    TotalSize = long.Parse(rootDisk[0]) / 1024,
                    Used = long.Parse(rootDisk[1]) / 1024,
                    AvailableFreeSpace = long.Parse(rootDisk[2]) / 1024,
                    AvailablePercent = decimal.Parse(rootDisk[3].Replace("%", ""))
                };
                diskInfos.Add(diskInfo);
            }
            else
            {
                var driv = DriveInfo.GetDrives().Where(u => u.IsReady);
                foreach (var item in driv)
                {
                    if (item.DriveType == DriveType.CDRom) continue;
                    var obj = new DiskInfo()
                    {
                        DiskName = item.Name,
                        TypeName = item.DriveType.ToString(),
                        TotalSize = item.TotalSize / 1024 / 1024 / 1024,
                        AvailableFreeSpace = item.AvailableFreeSpace / 1024 / 1024 / 1024,
                    };
                    obj.Used = obj.TotalSize - obj.AvailableFreeSpace;
                    obj.AvailablePercent = decimal.Ceiling(obj.Used / (decimal)obj.TotalSize * 100);
                    diskInfos.Add(obj);
                }
            }
            return diskInfos;
        }

        /// <summary>
        /// 判断房钱运行平台是否是Unix
        /// </summary>
        /// <returns></returns>
        public static bool IsUnix()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        /// <summary>
        /// 获取CPU
        /// </summary>
        /// <returns></returns>
        public static string GetCPURate()
        {
            string cpuRate;
            if (IsUnix())
            {
                cpuRate = GetUnixCPURateByTop();
                if (string.IsNullOrEmpty(cpuRate))
                {
                    cpuRate = GetUnixCPURateByStat();
                }
            }
            else
            {
                string output = ShellUtil.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }
            return cpuRate;
        }

        /// <summary>
        /// 通过top命令获取CPU使用率
        /// </summary>
        /// <returns></returns>
        private static string GetUnixCPURateByTop()
        {
            try
            {
                string output = ShellUtil.Bash("top -bn2 -d.2");
                // 解析输出并提取 CPU 使用率
                string[] lines = output.Split(new string[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries);

                string cpuUsageLine = lines.FirstOrDefault(line => line.StartsWith("%Cpu(s):"));
                if (cpuUsageLine != null)
                {
                    string[] fields = cpuUsageLine.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var field = fields.FirstOrDefault(r => r.Contains("id"));
                    if (!string.IsNullOrWhiteSpace(field))
                    {
                        string[] idleFields = field.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        if (idleFields.Length >= 3)
                        {
                            double idlePercentage = double.Parse(idleFields[0]);

                            // 计算 CPU 使用率
                            //// 输出 CPU 使用率
                            //Console.WriteLine($"CPU Usage: {cpuUsage:0.##}%");
                            return (100 - idlePercentage).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"使用top 命令获取cpu使用率失败，异常消息：{ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// 通过/proc/stat文件获取CPU使用率
        /// </summary>
        /// <returns></returns>
        private static string GetUnixCPURateByStat()
        {
            try
            {
                // 读取 /proc/stat 文件
                string statFile = "/proc/stat";
                string[] lines = File.ReadAllLines(statFile);

                // 查找以 "cpu" 开头的行（即每个 CPU 核心的统计信息）
                var cpuLines = lines.Where(line => line.StartsWith("cpu"));

                foreach (string cpuLine in cpuLines)
                {
                    string[] fields = cpuLine.Split(' ');

                    // 解析 CPU 统计信息
                    string cpuName = fields[0];
                    long.TryParse(fields[1], out long userTime);
                    long.TryParse(fields[2], out long niceTime);
                    long.TryParse(fields[3], out long systemTime);
                    long.TryParse(fields[4], out long idleTime);

                    // 计算 CPU 使用率
                    long totalTime = userTime + niceTime + systemTime + idleTime;
                    double usagePercent = ((double)(totalTime - idleTime) / totalTime) * 100;
                    return usagePercent.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"使用/proc/stat 命令获取cpu使用率失败，异常消息：{ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// 获取系统运行时间
        /// </summary>
        /// <returns></returns>
        public static string GetRunTime()
        {
            string runTime = string.Empty;
            if (IsUnix())
            {
                string output = ShellUtil.Bash("uptime -s").Trim();
                DateTime.TryParse(output, out DateTime outputDate);
                var secondsStr = (DateTime.Now - outputDate).TotalMilliseconds.ToString().Split('.')[0];
                long.TryParse(secondsStr, out long seconds);
                runTime = DateTimeHelper.FormatTime(seconds);
            }
            else
            {
                string output = ShellUtil.Cmd("wmic", "OS get LastBootUpTime/Value");
                string[] outputArr = output.Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
                if (outputArr.Length == 2)
                {
                    var outputDateStr = outputArr[1].Split('.')[0];
                    DateTime.TryParse(outputDateStr, out DateTime outputDate);
                    var secondsStr = (DateTime.Now - outputDate).TotalMilliseconds.ToString().Split('.')[0];
                    long.TryParse(secondsStr, out long seconds);
                    runTime = DateTimeHelper.FormatTime(seconds);
                }

            }
            return runTime;
        }

        /// <summary>
        /// 获取外网IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIpFromOnline()
        {
            var url = "http://myip.ipip.net";
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
                {
                    var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                    var streamReader = new StreamReader(stream);
                    var html = streamReader.ReadToEnd();
                    return html.Replace("当前 IP：", "").Replace("来自于：", "");
                }
            }
        }
    }

    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryMetrics
    {
        internal double Total { get; set; }

        internal double Used { get; set; }

        internal double Free { get; set; }

        /// <summary>
        /// 已用内存
        /// </summary>
        public string UsedRam { get; set; }

        /// <summary>
        /// CPU使用率%
        /// </summary>
        public string CpuRate { get; set; }

        /// <summary>
        /// 总内存 GB
        /// </summary>
        public string TotalRam { get; set; }

        /// <summary>
        /// 内存使用率 %
        /// </summary>
        public string RamRate { get; set; }

        /// <summary>
        /// 空闲内存
        /// </summary>
        public string FreeRam { get; set; }
    }

    /// <summary>
    /// 磁盘信息
    /// </summary>
    public class DiskInfo
    {
        /// <summary>
        /// 磁盘名
        /// </summary>
        public string DiskName { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 总剩余
        /// </summary>
        public long TotalFree { get; set; }

        /// <summary>
        /// 总量
        /// </summary>
        public long TotalSize { get; set; }

        /// <summary>
        /// 已使用
        /// </summary>
        public long Used { get; set; }

        /// <summary>
        /// 可使用
        /// </summary>
        public long AvailableFreeSpace { get; set; }

        /// <summary>
        /// 使用百分比
        /// </summary>
        public decimal AvailablePercent { get; set; }
    }


    internal class MemoryMetricsClient
    {
        /// <summary>
        /// windows系统获取内存信息
        /// </summary>
        /// <returns></returns>
        public MemoryMetrics GetWindowsMetrics()
        {
            string output = ShellUtil.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
            var metrics = new MemoryMetrics();
            var lines = output.Trim().Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 0) return metrics;

            var freeMemoryParts = lines[0].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);

            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);//m
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        /// <summary>
        /// Unix系统获取
        /// </summary>
        /// <returns></returns>
        public MemoryMetrics GetUnixMetrics()
        {
            string output = ShellUtil.Bash("free -m | awk '{print $2,$3,$4,$5,$6}'");
            var metrics = new MemoryMetrics();
            var lines = output.Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 0) return metrics;

            if (lines != null && lines.Length > 0)
            {
                var memory = lines[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                if (memory.Length >= 3)
                {
                    metrics.Total = double.Parse(memory[0]);
                    metrics.Used = double.Parse(memory[1]);
                    metrics.Free = double.Parse(memory[2]);//m
                }
            }
            return metrics;
        }
    }

   
    internal class ShellUtil
    {
        /// <summary>
        /// linux 系统命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string Bash(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }

        /// <summary>
        /// windows系统命令
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Cmd(string fileName, string args)
        {
            string output = string.Empty;

            var info = new ProcessStartInfo();
            info.FileName = fileName;
            info.Arguments = args;
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }
            return output;
        }
    }
}
