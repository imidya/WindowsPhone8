
from lxml import etree
from lxml import objectify
import sys
reload(sys)
sys.setdefaultencoding("utf-8") # for handling chinese characters

if __name__ == '__main__':
	doc = objectify.parse('strings.xml')
	root = doc.getroot()
	for el in root.string:
		print el.get("name") + '\t' + el