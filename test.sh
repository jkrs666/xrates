docker run --rm --name redis_test -d -p 7777:6379 redis:8-alpine
cd ./Backend/test/XratesTests/
dotnet test -v diag --collect:"XPlat Code Coverage" --settings ../../../coverlet.runsettings
docker stop redis_test
