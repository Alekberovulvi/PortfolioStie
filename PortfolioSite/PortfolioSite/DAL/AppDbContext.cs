using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioSite.Models;
using PortfolioSite.ViewsModel;
using System.Diagnostics;

namespace PortfolioSite.DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base (options) { }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<SIcons> SIcons { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<AboutsSite> AboutsSites { get; set; }
        public DbSet<Percent> Percents { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Testimonials> Testimonials { get; set; }
        public DbSet<TestimonialsEdit> TestimonialsEdits { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactLocation> ContactLocations { get; set; }
        public DbSet<ContactFormModel> contactFormModels { get; set; }
        public DbSet<Cv> Cvs { get; set; }
        public DbSet<CVmodel> CVmodels { get; set; }
    }
}
