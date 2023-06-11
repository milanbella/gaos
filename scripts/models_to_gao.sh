#set -xe
GAO_PATH=../../gao

rm -rf $GAO_PATH/Assets/Scripts/Gaos/Model
cp -r ../Model $GAO_PATH/Assets/Scripts/Gaos/Model

rm -rf $GAO_PATH/Assets/Scripts/Gaos/Dbo/Model
cp -r ../Dbo/Model $GAO_PATH/Assets/Scripts/Gaos/Dbo/Model

rm -rf $GAO_PATH/Assets/Scripts/Gaos/Routes/Model
cp -r ../Routes/Model $GAO_PATH/Assets/Scripts/Gaos/Routes/Model

function edit_model_file()
{
  local model_file=$1
  #sed -n '/{[ ]*get[ ]*;[ ]*set;[ ]*}/p' $model_file 
  sed -i  's/[ ]*{[ ]*get[ ]*;[ ]*set[ ]*;[ ]*}[ ]*/;/' $model_file 
}

function get_model_files()
{
  local model_folder_path=$1
  echo $folder_path

  find $model_folder_path  -name "*.cs" -print
}

function edit_model_files()
{
  local model_files=$1

  for model_file in $model_files
  do
    #echo $model_file
    edit_model_file $model_file
  done
}

function edit_model_folder()
{
  local model_folder_path=$1

  local model_files=$(get_model_files $model_folder_path)
  edit_model_files "$model_files"
}

function edit_model_folders()
{
  local model_folders="$*"
  for model_folder in $model_folders
  do
    edit_model_folder $model_folder
  done
}

#edit_model_folder $GAO_PATH/Assets/Scripts/Gaos/Dbo/Model

edit_model_folders \
  $GAO_PATH/Assets/Scripts/Gaos/Model \
  $GAO_PATH/Assets/Scripts/Gaos/Dbo/Model \
  $GAO_PATH/Assets/Scripts/Gaos/Routes/Model




