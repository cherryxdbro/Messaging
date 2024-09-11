FROM postgres:latest AS runtime
ENTRYPOINT ["/bin/bash", "-c", "docker-entrypoint.sh postgres & \
    until pg_isready -p $PGPORT -U $POSTGRES_USER; do \
        echo 'Waiting for PostgreSQL to be ready...'; \
        sleep 2; \
    done; \
    echo 'PostgreSQL is ready. Running the initialization script...'; \
    PGPASSWORD=$(cat /run/secrets/database-password) psql -d $POSTGRES_DB -f /run/secrets/initialize-script -p $PGPORT -U $POSTGRES_USER; \
    fg"]