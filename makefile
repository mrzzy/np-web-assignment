#
# NP Web Assignment
# Project Makefile 
# Progates changes made in project configration to all projects
#

TARGET_PROJECTS:=folio folio_tests
TARGET_CONFIG:=.env
TARGET_PATHS:=$(foreach p,$(TARGET_PROJECTS),$(p)/$(TARGET_CONFIG))

# targets
.DEFAULT: all
.PHONY: all

all: $(TARGET_PATHS)

# file targets
folio/%: %
	cp -af $< $@

folio_tests/%: %
	cp -af $< $@
