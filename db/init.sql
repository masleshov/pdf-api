create extension if not exists "uuid-ossp";

create table customer
(
    customer_id serial primary key,
    email varchar(255) not null,
    password_hash varchar(255) not null,
    password_salt varchar(255) not null,
    status integer not null default 0,
    create_timestamp timestamp not null default (current_timestamp at time zone 'UTC'),
    update_timestamp timestamp not null default (current_timestamp at time zone 'UTC')
);

create table email_confirmation
(
    confirmation_id uuid primary key default uuid_generate_v4(),
    customer_id integer not null references customer(customer_id),
    email varchar(255) not null,
    code varchar(8) not null,
    expired timestamp not null,
    attempts integer not null default 0,
    status integer not null default 0,
    create_timestamp timestamp not null default (current_timestamp at time zone 'UTC'),
    update_timestamp timestamp not null default (current_timestamp at time zone 'UTC')
);

create table authorization_info
(
    customer_id integer primary key references customer(customer_id),
    access_token text not null,
    refresh_token text not null,
    expired timestamp not null,
    create_timestamp timestamp not null default (current_timestamp at time zone 'UTC'),
    update_timestamp timestamp not null default (current_timestamp at time zone 'UTC')
)