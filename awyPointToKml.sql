USE aviation;

SELECT DISTINCT pointName,pointType,latitude,longitude
	INTO OUTFILE 'C:\\Users\\junk_\\Documents\\qgisBatch\\awyPointToKml.txt' FIELDS TERMINATED BY '~' LINES TERMINATED BY '\r\n'
	FROM airway
	WHERE type=@type ORDER BY pointName;
