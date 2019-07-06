#
# NP Web Assignment
# Project Makefile 
# Progates changes made in project configration to all projects
#

TARGET_PROJECTS:=folio folio_tests folio_ui
TARGET_CONFIG:=.env
TARGET_PATHS:=$(foreach p,$(TARGET_PROJECTS),$(p)/$(TARGET_CONFIG))

# targets
.DEFAULT: all
.PHONY: all test test/api

all: $(TARGET_PATHS)

# test api using newman
test: test/api

test/api:
	newman run folio_tests/API/folio-api.postman_collection.json

# file targets
folio/%: %
	cp -af $< $@

folio_tests/%: %
	cp -af $< $@

folio_ui/%: %
	cp -af $< $@

clean:
	rm -f $(TARGET_PATHS)
