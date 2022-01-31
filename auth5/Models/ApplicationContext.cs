using Microsoft.EntityFrameworkCore;

namespace auth5.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ItemCollection> Collections { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@email.com";
            string adminPassword = "123456";
            string userEmail = "user@email.com";
            string userPassword = "qwerty";

            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User 
            { 
                Id = 1, 
                Name = "Admin", 
                Email = adminEmail, 
                Password = adminPassword, 
                RoleId = adminRole.Id,
                CreatedDate =  DateTime.Now, 
                Status = "Immune"
            };
            User normalUser = new User 
            { 
                Id = 2, 
                Name = "User", 
                Email = userEmail, 
                Password = userPassword, 
                RoleId = userRole.Id,
                CreatedDate = DateTime.Now, 
                Status = "Normal"
            };
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, normalUser });
            modelBuilder.Entity<ItemCollection>(c => {
                c.HasData(new {
                    Id = 1,
                    OwnerEmail = adminEmail,
                    Title = "Collection #1",
                    Theme = "Books",
                    Description = "Book Collection",
                    ImgSrc = "",
                    StrFields = "Author",
                    TextFields = "Description",
                    NumFields = "Pages|Chapters",
                    BoolFields = "Owned"
                });
                c.OwnsMany(x => x.Items).HasData(new[] { 
                    new {
                        Id = 1,
                        ItemCollectionId = 1,
                        Title = "Green Mile",
                        StrFields = "Stephen King",
                        TextFields = "1996 serial novel by American writer Stephen King",
                        NumFields = "432|6",
                        BoolFields = "True",
                        Comments = "",
                        Likes = ""
                    },
                    new {
                        Id = 2,
                        ItemCollectionId = 1,
                        Title = "Lord of the Flies",
                        StrFields = "William Golding",
                        TextFields = "1954 debut novel by Nobel Prize-winning British author William Golding",
                        NumFields = "224|12",
                        BoolFields = "False",
                        Comments = "",
                        Likes = ""
                    },
                    new {
                        Id = 3,
                        ItemCollectionId = 1,
                        Title = "The Hobbit",
                        StrFields = "J.R.R. Tolkien",
                        TextFields = "The first of J.R.R. Tolkien published books",
                        NumFields = "304|19",
                        BoolFields = "False",
                        Comments = "",
                        Likes = ""
                    },
                });
            });
            modelBuilder.Entity<ItemCollection>(c => {
                c.HasData(new
                {
                    Id = 2,
                    OwnerEmail = userEmail,
                    Title = "Collection #2",
                    Theme = "Movies",
                    Description = "Movie Collection",
                    ImgSrc = "",
                    StrFields = "Director",
                    TextFields = "Description|PG Rating",
                    NumFields = "Length(minutes)",
                    BoolFields = "Owned"
                });
                c.OwnsMany(x => x.Items).HasData(new[] {
                    new {
                        Id = 4,
                        ItemCollectionId = 2,
                        Title = "Spider-Man 3",
                        StrFields = "Sam Raimi",
                        TextFields = "2007 American superhero film based on the Marvel Comics character Spider-Man|PG-13",
                        NumFields = "139",
                        BoolFields = "True",
                        Comments = "",
                        Likes = ""
                    },
                    new {
                        Id = 5,
                        ItemCollectionId = 2,
                        Title = "The Room",
                        StrFields = "Tommy Wiseau",
                        TextFields = "2003 American independent drama film written, produced, executive produced and directed by Tommy Wiseau|R",
                        NumFields = "99",
                        BoolFields = "True",
                        Comments = "",
                        Likes = ""
                    }
                });
            });
            modelBuilder.Entity<ItemCollection>(c => {
                c.HasData(new
                {
                    Id = 3,
                    OwnerEmail = userEmail,
                    Title = "Collection #3",
                    Theme = "Games",
                    Description = "Game Collection",
                    ImgSrc = "",
                    StrFields = "Studio|Platform",
                    TextFields = "Description",
                    NumFields = "",
                    BoolFields = "Owned|Available"
                });
                c.OwnsMany(x => x.Items).HasData(new[] {
                    new {
                        Id = 6,
                        ItemCollectionId = 3,
                        Title = "God Hand",
                        NumFields = "",
                        StrFields = "Clover Studio|PS2",
                        TextFields = "PlayStation 2 action beat 'em up video game developed by Clover Studio and published by Capcom",
                        BoolFields = "False|False",
                        Comments = "",
                        Likes = ""
                    },
                    new {
                        Id = 7,
                        ItemCollectionId = 3,
                        Title = "Psychonauts",
                        NumFields = "",
                        StrFields = "Double Fine Productions|Windows,Xbox,PS2,Mac OS X,Linux",
                        TextFields = "2005 platform video game developed by Double Fine Productions",
                        BoolFields = "True|True",
                        Comments = "",
                        Likes = ""
                    },
                    new {
                        Id = 8,
                        ItemCollectionId = 3,
                        Title = "Deponia",
                        NumFields = "",
                        StrFields = "Daedalic Entertainment|Windows,Mac OS X,Linux,iOS,PS4,Nintendo Switch,Xbox One",
                        TextFields = "Graphic adventure video game developed and published by Daedalic Entertainment",
                        BoolFields = "False|True",
                        Comments = "",
                        Likes = ""
                    },
                    new {
                        Id = 9,
                        ItemCollectionId = 3,
                        Title = "Yakuza 0",
                        NumFields = "",
                        StrFields = "Ryu Ga Gotoku Studio|PS3,PS4,Windows,Xbox One,Amazon Luna",
                        TextFields = "Action-adventure game developed and published by Sega",
                        BoolFields = "True|True",
                        Comments = "",
                        Likes = ""
                    }
                });
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
