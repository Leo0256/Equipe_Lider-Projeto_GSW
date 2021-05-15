create database "GSWSystem"
    with 
    owner = postgres
    encoding = 'UTF8'
    lc_collate = 'Portuguese_Brazil.1252'
    lc_ctype = 'Portuguese_Brazil.1252'
    tablespace = pg_default
    connection limit = -1;


create table funcionarios(
    id_func varchar(40) primary key,
    primeiro_nome varchar(50) not null,
    ultimo_nome varchar(50) not null,
    avatar varchar(100) default 'http://placeimg.com/640/480/people',
    email varchar(100) not null
)


create table gitmetadata(
    id_git serial primary key,
    branch varchar(50) not null,
    hash varchar(50) unique not null
)


create table projeto_info(
    id_info serial primary key,
    nome varchar(100) not null,
    descr varchar(200),
    horas numeric(4,2) default 0.0
)


create table projeto(
    id varchar(40) primary key,
    id_func varchar(40) not null,
    id_info integer not null,
    id_git integer not null,
    iniciado timestamp,
    status varchar(20),
    finalizado integer default 0,
    foreign key (id_func) references funcionarios (id_func),
    foreign key (id_info) references projeto_info (id_info),
    foreign key (id_git) references gitmetadata (id_git)
)


create function pesquisa_dedicacao_func (nome_projeto varchar) 
returns table (
	projeto varchar,
	p_nome varchar,
	u_nome varchar,
	porcentagem double precision
) language plpgsql
as $$
begin
	return query 
		select
			proj.nome,
			func.p_nome,
			func.u_nome,
			((func.horas/proj.horas::float)*100)::float as valor

		from 
			(
				select
					funcionarios.primeiro_nome as p_nome,
					funcionarios.ultimo_nome as u_nome,
					sum(projeto_info.horas) as horas

				from funcionarios 
					inner join projeto
						on funcionarios.id_func = projeto.id_func
					inner join projeto_info
						on projeto.id_info = projeto_info.id_info
				where 
					projeto_info.nome ilike concat('%',nome_projeto,'%')

				group by funcionarios.primeiro_nome,funcionarios.ultimo_nome

			)as func,
			(
				select
					pesquisa_horas_projeto.projeto as nome,
					horas
				from pesquisa_horas_projeto()
				where 
					pesquisa_horas_projeto.projeto ilike concat('%',nome_projeto,'%')

			)as proj

		group by proj.nome,func.p_nome,func.u_nome,valor
		order by proj.nome desc;
end; $$


create function pesquisa_finalizado_projeto (finalizado_x integer) 
returns table (
	id varchar,
	id_func varchar,
	nome varchar,
	decr varchar,
	iniciado timestamp,
	status varchar,
	finalizado integer
) language plpgsql
as $$
begin
	return query 
		select
			projeto.id, 
			projeto.id_func, 
			projeto_info.nome,
			projeto_info.descr,
			projeto.iniciado,
			projeto.status,
			projeto.finalizado
		from projeto, projeto_info
		where 
			projeto.id_info = projeto_info.id_info
		and
			projeto.finalizado = finalizado_x
		order by projeto.finalizado asc;

end; $$


create function pesquisa_horas_ano (ano_x integer) 
returns table (
	ano double precision,
	horas numeric(4,2)
) language plpgsql
as $$
begin
	return query 
		select
			extract(year from projeto.iniciado),
			sum(projeto_info.horas)

		from projeto_info
			inner join projeto 
				on projeto_info.id_info = projeto.id_info
				
		where 
			extract(year from projeto.iniciado)::integer = ano_x
		
		group by extract(year from projeto.iniciado)
		order by ano_x desc;

end; $$


create function pesquisa_horas_funcionario (func_nome varchar) 
returns table (
	id_func varchar,
	horas numeric(5,2),
	primeiro_nome varchar,
	ultimo_nome varchar,
	avatar varchar,
	email varchar
) language plpgsql
as $$
begin
	return query 
		select
			funcionarios.id_func,
			sum(projeto_info.horas) as horas,
			funcionarios.primeiro_nome,
			funcionarios.ultimo_nome,
			funcionarios.avatar,
			funcionarios.email
	
		from funcionarios
			inner join projeto 
				on funcionarios.id_func = projeto.id_func
			inner join projeto_info 
				on projeto.id_info = projeto_info.id_info

		where
			funcionarios.primeiro_nome ilike concat('%',func_nome,'%') 
		or
			funcionarios.ultimo_nome ilike concat('%',func_nome,'%')
	
		group by funcionarios.id_func
		order by funcionarios.primeiro_nome desc;
end; $$


create function pesquisa_horas_mes (mes_x integer, ano_x integer) 
returns table (
	mes double precision,
	horas numeric(4,2)
) language plpgsql
as $$
begin
	return query 
		select
			extract(month from projeto.iniciado),
			sum(projeto_info.horas)

		from projeto_info
			inner join projeto 
				on projeto_info.id_info = projeto.id_info
				
		where 
			extract(month from projeto.iniciado)::integer = mes_x
		and
			extract(year from projeto.iniciado)::integer = ano_x
		
		group by extract(month from projeto.iniciado)
		order by mes_x desc;

end; $$


create function pesquisa_horas_projeto() 
returns table (
	projeto varchar,
	horas numeric
) language plpgsql
as $$
begin
	return query 
		select
			projeto_info.nome,
			sum(projeto_info.horas)

		from projeto_info
			inner join projeto 
				on projeto_info.id_info = projeto.id_info
				
		group by projeto_info.nome
		order by projeto_info.nome desc;
end; $$


create function pesquisa_nome_projeto (nome_x varchar) 
returns table (
	id varchar,
	id_func varchar,
	nome varchar,
	decr varchar,
	iniciado timestamp,
	status varchar,
	finalizado integer
) language plpgsql
as $$
begin
	return query 
		select
			projeto.id as id, 
			projeto.id_func as id_func, 
			projeto_info.nome as nome,
			projeto_info.descr as descr,
			projeto.iniciado as iniciado,
			projeto.status as status,
			projeto.finalizado as finalizado
		from projeto 
			inner join projeto_info
				on projeto.id_info = projeto_info.id_info
		where 
			projeto_info.nome ilike concat('%',nome_x,'%')
		order by nome desc;

end; $$


create function pesquisa_quant_status () 
returns table (
	status varchar,
	total bigint
) language plpgsql
as $$
begin
	return query 
		select
			projeto.status,
			count(projeto.status)
		from projeto
		group by projeto.status
		order by projeto.status desc;

end; $$


create function pesquisa_status_projeto (status_x varchar) 
returns table (
	id varchar,
	id_func varchar,
	nome varchar,
	decr varchar,
	iniciado timestamp,
	status varchar,
	finalizado integer
) language plpgsql
as $$
begin
	return query 
		select
			projeto.id, 
			projeto.id_func, 
			projeto_info.nome,
			projeto_info.descr,
			projeto.iniciado,
			projeto.status,
			projeto.finalizado
		from projeto, projeto_info
		where 
			projeto.id_info = projeto_info.id_info
		and
			projeto.status = concat(status_x,'%')
		order by projeto_info.nome desc;

end; $$


create function pesquisa_tasks_abertas() 
returns table (
	id varchar,
	projeto varchar,
	status varchar
) language plpgsql
as $$
begin
	return query
		select
			projeto.id,
			projeto_info.nome,
			projeto.status
			
		from projeto
			inner join projeto_info
				on projeto.id_info = projeto_info.id_info
			
		where
			projeto.finalizado = 0
			
		group by 
			projeto.id,
			projeto_info.nome,
			projeto.status
			
		order by projeto_info.nome desc;
end; $$