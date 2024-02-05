DEL params.sql

REM DEL awyPoint*.kml

DEL awyPointToKml.txt
DEL awyPointToKmlNavaids.txt

mysql.exe --login-path=batch --table < awyPointToKmlNavaids.sql

REM select for type
REM SET type=A
REM SET type=B
REM SET type=G
REM SET type=J
REM SET type=Q
REM SET type=R
REM SET type=T
SET type=V
REM SET type=H
REM SET type=L
REM SET type=M
REM SET type=N
REM SET type=W
REM SET type=Y

ECHO SET @type='%type%'; > params.sql
TYPE awyPointToKml.sql >> params.sql

mysql.exe --login-path=batch --table < params.sql

awyPointToKml.exe awyPointToKmlNavaids.txt %type% awyPointToKml.txt

REM DEL awyPointToKml.txt
REM DEL awyPointToKmlNavaids.txt

DEL params.sql

REM SET GDAL_DATA=C:\\Program Files\\QGIS 3.22.1\\apps\\gdal-dev\\data
REM SET GDAL_DRIVER_PATH=C:\\Program Files\\QGIS 3.22.1\\bin\\gdalplugins
REM SET OSGEO4W_ROOT=C:\\Program Files\\QGIS 3.22.1
REM SET PATH=%OSGEO4W_ROOT%\\bin;%PATH%
REM SET PYTHONHOME=%OSGEO4W_ROOT%\\apps\\Python37
REM SET PYTHONPATH=%OSGEO4W_ROOT%\\apps\\Python37

REM ogr2ogr.exe -f "ESRI Shapefile" -skipfailures "shape\\fixPoint%type%.shp" "fixPoint.kml" fixPoint
REM ogr2ogr.exe -f "ESRI Shapefile" -skipfailures "shape\\ndbPoint%type%.shp" "ndbPoint.kml" ndbPoint
REM ogr2ogr.exe -f "ESRI Shapefile" -skipfailures "shape\\vorDmePoint%type%.shp" "vorDmePoint.kml" vorDmePoint
REM ogr2ogr.exe -f "ESRI Shapefile" -skipfailures "shape\\vorPoint%type%.shp" "vorPoint.kml" vorPoint
REM ogr2ogr.exe -f "ESRI Shapefile" -skipfailures "shape\\vortacPoint%type%.shp" "vortacPoint.kml" vortacPoint
