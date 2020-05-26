<?php

$username = $_POST["username"];
$itemnumber = $_POST["itemnumber"];
$itempay = $_POST["itempay"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

if($itemnumber === 'item1'){
$sql = "UPDATE item SET item1=(1)  WHERE username ='".$username."'";
}
else if ($itemnumber === 'item2'){
  $sql = "UPDATE item SET item2=(1)  WHERE username ='".$username."'";
}
else if ($itemnumber === 'item3'){
  $sql = "UPDATE item SET item3=(1)  WHERE username ='".$username."'";
}
else if ($itemnumber === 'item4'){
  $sql = "UPDATE item SET item4=(1)  WHERE username ='".$username."'";
}


$result = $conn->query($sql) or die("2 : query failed");

$sql2 = "UPDATE player SET heart=(heart-'".$itempay."') WHERE username ='".$username."'";
$result2 = $conn->query($sql2) or die("2 : query failed");

$conn->close();

?>
