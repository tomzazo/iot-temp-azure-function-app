.PHONY: init test deploy
.SILENT: init test deploy

init:
	terraform init

test: init
	terraform validate

deploy: init
	terraform apply