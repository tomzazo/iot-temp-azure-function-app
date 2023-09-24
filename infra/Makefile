.PHONY: deploy

init:
	terraform init

test: init
	terraform validate

deploy: init
	terraform apply