Run following command from Nuget package manager console (tools->Nuget Package manager->package manager console )

Step 1
dotnet tool install --global dotnet-ef

Step 2
******
dotnet ef dbcontext scaffold "server=localhost;port=3306;user=root;password=Mdb123;database=btcoreframeworkdb" Pomelo.EntityFrameworkCore.MySql -c AppDBContext -o DBModels -f --project AppDAL


Change password and databse if require