[ ! -s .env ] && cp .env.example .env
docker compose down -v
docker compose up -d --build --force-recreate
