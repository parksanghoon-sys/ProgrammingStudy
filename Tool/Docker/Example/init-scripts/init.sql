
-- 관리자에게 슈퍼유저 권한 부여
ALTER ROLE admin WITH SUPERUSER;

-- 일반 사용자 생성
CREATE ROLE app_user WITH LOGIN PASSWORD '123456';

-- 일반 사용자용 스키마 생성
CREATE SCHEMA app_schema;

-- app_user에게 app_schema에 대한 모든 권한 부여
GRANT ALL PRIVILEGES ON SCHEMA app_schema TO app_user;

-- 애플리케이션용 데이터베이스 생성
CREATE DATABASE app_db;

-- 데이터베이스 연결 권한 부여
GRANT CONNECT ON DATABASE app_db TO app_user;
GRANT CONNECT ON DATABASE app_db TO admin;

-- 새로 생성한 데이터베이스로 연결
\c app_db

-- app_schema 생성
CREATE SCHEMA app_schema;

-- app_user에게 app_schema에 대한 모든 권한 부여
GRANT ALL PRIVILEGES ON SCHEMA app_schema TO app_user;

-- 테이블 예시 생성
CREATE TABLE app_schema.users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- app_user에게 테이블에 대한 권한 부여
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA app_schema TO app_user;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA app_schema TO app_user;

-- 추후 생성될 테이블에 대한 권한도 기본적으로 부여
ALTER DEFAULT PRIVILEGES IN SCHEMA app_schema 
GRANT ALL PRIVILEGES ON TABLES TO app_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA app_schema 
GRANT USAGE, SELECT ON SEQUENCES TO app_user;