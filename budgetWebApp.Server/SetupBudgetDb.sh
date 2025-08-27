#!/bin/bash

# Connection details
SERVER="(localdb)\\development"
DATABASE="Budget"
CREATE_SCRIPT="DbSetup.sql"
DATA_SCRIPT="AddTestData.sql"

# Step 1: Create the database if it doesn't exist
echo "Creating database '$DATABASE'..."
sqlcmd -S "$SERVER" -Q "IF DB_ID('$DATABASE') IS NULL CREATE DATABASE [$DATABASE];"

# Step 2: Run the schema creation script
echo "Running schema script..."
sqlcmd -S "$SERVER" -d "$DATABASE" -i "$CREATE_SCRIPT"

# Step 3: Run the test data script
echo "Inserting test data..."
sqlcmd -S "$SERVER" -d "$DATABASE" -i "$DATA_SCRIPT"

echo "✅ Setup complete."