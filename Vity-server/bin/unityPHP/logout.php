<?php

$username = $_POST["username"];
$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

$sql = "UPDATE player SET login=(0) WHERE username ='".$username."'";
$result = $conn->query($sql);

?>
