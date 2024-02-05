USE aviation;

SELECT facilityId,type,freq,name
	INTO OUTFILE 'C:\\Users\\junk_\\Documents\\qgisBatch\\awyPointToKmlNavaids.txt' FIELDS TERMINATED BY '~' LINES TERMINATED BY '\r\n'
	FROM navnavaid;
