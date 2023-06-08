set -xe
GAO_PATH=../../gao

rm -rf $GAO_PATH/Assets/Scripts/Gaos/Model
cp -r ../Model $GAO_PATH/Assets/Scripts/Gaos

rm -rf $GAO_PATH/Assets/Scripts/Gaos/Dbo/Model
cp -r ../Dbo/Model $GAO_PATH/Assets/Scripts/Gaos/Dbo

rm -rf $GAO_PATH/Assets/Scripts/Gaos/Routes/Model
cp -r ../Routes/Model $GAO_PATH/Assets/Scripts/Gaos/Routes/Model
