#!/bin/bash
set -e

# private key
set -x
openssl genrsa -out out/key.pem 4096
set +x

# public key (no password protected)
set -x
openssl req -new -x509 -key out/key.pem -out out/cert.crt -days 90000 
set +x

# put public and private key in new pkcs12 keystore 
set -x
openssl pkcs12 -export -out out/key_store.pfx -inkey out/key.pem -in out/cert.crt
set +x

# to list the keystore (you must specify password to encrypt the displayed private key)
# openssl pkcs12 -info -in out/key_store.pfx

# to list the keystore (displayed private key is not encrypted by password)
# openssl pkcs12 -info -in out/key_store.pfx -nodes
