[ ! -s .env ] && cp .env.example .env
source .env
export ConnectionStrings__Postgres="Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=$POSTGRES_PASSWORD"
cd Xrates &&
dotnet ef database update
cd -
