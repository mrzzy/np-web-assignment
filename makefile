#
# NP Web Assignment
# Project Makefile 
# Progates changes made in project configration to all projects
#

TARGET_PROJECTS:=folio folio_tests
TARGET_CONFIG:=appsettings.json .env
# TODO: figure out a way to generate this with nested for loop
TARGET_PATHS:=folio/appsettings.json folio_tests/appsettings.json folio/.env folio_test.env

# targets
.DEFAULT: all
.PHONY: all

all: $(TARGET_PATHS)

# file targets
folio/%: %
	cp -af $< $@

folio_tests/%: %
	cp -af $< $@
