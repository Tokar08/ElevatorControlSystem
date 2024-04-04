using GrainElevatorCS_ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS_ef;

static class OptionsManager
{
    public static DbContextOptions GetOptions()
    {
        try
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("config.json");

            var config = builder.Build();
            string? connString = config.GetConnectionString("Express");

            DbContextOptionsBuilder<Db> optBuilder = new DbContextOptionsBuilder<Db>();
            optBuilder.UseSqlServer(connString);

            return optBuilder.Options;
        }
        catch (Exception)
        {
            //TODO
            throw;
        }
    }

}
