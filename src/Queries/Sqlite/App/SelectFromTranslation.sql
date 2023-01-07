SELECT t.english, t.{0} 
FROM translation t 
WHERE UPPER(t.context) LIKE UPPER('{1}'); 
