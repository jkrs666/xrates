docker run --rm --name redis_test -d -p 7777:6379 redis:8-alpine
docker run --rm --name pg_test -e POSTGRES_PASSWORD=pg_test -d -p 9999:5432 postgres:18-alpine
