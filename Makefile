.PHONY: deploy/infra deploy/fn

deploy/infra:
	$(MAKE) -C infra deploy

deploy/fn:
	$(MAKE) -C iot-temp-fn-app publish