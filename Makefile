.PHONY: test/infra build/fn deploy/infra deploy/fn
.SILENT: test/infra build/fn deploy/infra deploy/fn

test/infra:
	$(MAKE) -C infra test

build/fn:
	$(MAKE) -C iot-temp-fn-app build

deploy/infra:
	$(MAKE) -C infra deploy

deploy/fn:
	$(MAKE) -C iot-temp-fn-app publish