#
# NP Web Assignment
# Project Makefile 
# Progates changes made in project configration to all projects
#

ENV_TARGET_PROJECTS:=folio folio_tests folio_ui folio_ui/Views
ENV_TARGET_CONFIG:=.env
ENV_TARGET_PATHS:=$(foreach p,$(ENV_TARGET_PROJECTS),$(p)/$(ENV_TARGET_CONFIG))

DOCKER_TARGET_SUFFIXES:=db api ui
DOCKER_TARGET_IMAGES:=$(foreach s,$(DOCKER_TARGET_SUFFIXES),mrzzy/np_web_folio_$(s))
DOCKER_EXPORT_DIR:=containers
DOCKER_EXPORT_TGZ:=containers.tgz

# targets
.DEFAULT: all 
.PHONY: all test test/api clean env build export

all: env build export

# docker image targets
build:
	docker-compose build

export: $(DOCKER_TARGET_IMAGES)
	tar cvzf $(DOCKER_EXPORT_TGZ) $(DOCKER_EXPORT_DIR)

import: $(DOCKER_EXPORT_TGZ)
	tar xzvf $(DOCKER_EXPORT_TGZ)
	$(foreach i,$(DOCKER_TARGET_IMAGES),\
		docker load -i $(DOCKER_EXPORT_DIR)/$(notdir $i).tar;)


mrzzy/np_web_folio_%:
	@mkdir -p $(DOCKER_EXPORT_DIR)
	docker save --output=$(DOCKER_EXPORT_DIR)/$(notdir $@).tar $@

# file targets
# env files
env: $(ENV_TARGET_PATHS)

folio/%: %
	cp -af $< $@

folio_tests/%: %
	cp -af $< $@

folio_ui/%: %
	cp -af $< $@

folio_ui/Views/%: %
	cp -af $< $@

clean:
	rm -f $(ENV_TARGET_PATHS)
	rm -f $(DOCKER_EXPORT_DIR) $(DOCKER_EXPORT_TGZ)

# test api using newman
test: test/api

test/api:
	newman run folio_tests/API/folio-api.postman_collection.json

