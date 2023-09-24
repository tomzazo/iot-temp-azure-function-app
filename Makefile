.PHONY: deploy/infra deploy/fn

test/infra:
	$(MAKE) -C infra test

deploy/infra:
	$(MAKE) -C infra deploy

deploy/fn:
	$(MAKE) -C iot-temp-fn-app publish