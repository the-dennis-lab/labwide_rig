import os, sys, math
from datetime import datetime
import tifffile as tif
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.patches as patches
import pandas as pd
import utils

print(sys.argv)
print(len(sys.argv))
if len(sys.argv)<3:
    print('error, you must enter two integers, \n ls and ax3 position for the pole in the top of A1 \n (furthest from the door) ')
else:

    try:
        A1_top_ls = int(sys.argv[1])
        A1_top_ax= int(sys.argv[2])
    except:
        print('error, you must enter two integers, ls and ax3 position for the pole in the top of A1 (furthest from the door), you entered {}'.format(sys.argv[1:]))

    print('using {} as A1 top, ls \n using {} as A1 top, ax3 in so-called Native Zaber units'.format(A1_top_ls,A1_top_ax))

    
middle_from_0_offset = [8900,700]
A1c = (A1_top_ls+middle_from_0_offset[0],A1_top_ax+middle_from_0_offset[1])
print('a1c is {}'.format(A1c))
A_ls_cs,A_ax_cs=A1c
B3c = utils.get_neighbor_tile(A1c)
B_ls_cs,B_ax_cs = utils.get_row_vals(B3c,2,4)
print('B3c is {}'.format([B_ls_cs[2],B_ax_cs[2]]))
C3c=utils.get_neighbor_tile([B_ls_cs[0],B_ax_cs[0]])
C_ls_cs,C_ax_cs = utils.get_row_vals(C3c,2,7)
D3c=utils.get_neighbor_tile([C_ls_cs[0],C_ax_cs[0]])
D_ls_cs,D_ax_cs = utils.get_row_vals(D3c,2,10)
E3c=utils.get_neighbor_tile([D_ls_cs[0],D_ax_cs[0]])
E_ls_cs,E_ax_cs = utils.get_row_vals(E3c,2,13)
print('E1c is {}'.format([E_ls_cs[0],E_ax_cs[0]]))
print('E13c is {}'.format([E_ls_cs[12],E_ax_cs[12]]))
F1c=utils.get_neighbor_tile([E_ls_cs[0],E_ax_cs[0]])
F_ls_cs,F_ax_cs = utils.get_row_vals(F1c,0,12)
print('F1c is {}'.format([F_ls_cs[0],F_ax_cs[0]]))
G2c=utils.get_neighbor_tile([F_ls_cs[0],F_ax_cs[0]])
G_ls_cs,G_ax_cs = utils.get_row_vals(G2c,1,13)
H1c=utils.get_neighbor_tile([G_ls_cs[0],G_ax_cs[0]])
H_ls_cs,H_ax_cs = utils.get_row_vals(H1c,0,12)
I2c=utils.get_neighbor_tile([H_ls_cs[0],H_ax_cs[0]])
I_ls_cs,I_ax_cs = utils.get_row_vals(I2c,1,13)
J1c=utils.get_neighbor_tile([I_ls_cs[0],I_ax_cs[0]])
J_ls_cs,J_ax_cs = utils.get_row_vals(J1c,0,12)
K2c=utils.get_neighbor_tile([J_ls_cs[0],J_ax_cs[0]])
K_ls_cs,K_ax_cs = utils.get_row_vals(K2c,1,13)
L1c=utils.get_neighbor_tile([K_ls_cs[0],K_ax_cs[0]])
L_ls_cs,L_ax_cs = utils.get_row_vals(L1c,0,12)
M2c=utils.get_neighbor_tile([L_ls_cs[0],L_ax_cs[0]])
M_ls_cs,M_ax_cs = utils.get_row_vals(M2c,1,13)
print('M1c {}'.format([M_ls_cs[0],M_ax_cs[0]]))
print('M13c {}'.format([M_ls_cs[12],M_ax_cs[12]]))
N1c=utils.get_neighbor_tile([M_ls_cs[1],M_ax_cs[1]])
N_ls_cs,N_ax_cs = utils.get_row_vals(N1c,0,10)
# from N2
O1c=utils.get_neighbor_tile([N_ls_cs[1],N_ax_cs[1]])
O_ls_cs,O_ax_cs = utils.get_row_vals(O1c,0,7)
P1c= utils.get_neighbor_tile([O_ls_cs[1],O_ax_cs[1]])
P_ls_cs,P_ax_cs = utils.get_row_vals(P1c,0,4)
#from O2
Q1c= utils.get_neighbor_tile([P_ls_cs[1],P_ax_cs[1]])
Q_ls_cs,Q_ax_cs = Q1c
print('Q1c {}'.format(Q1c))


######################
# assemble, plot fig
all_ax_cs = [[A_ax_cs],B_ax_cs,C_ax_cs,D_ax_cs,E_ax_cs,F_ax_cs,G_ax_cs,H_ax_cs,I_ax_cs,J_ax_cs,K_ax_cs,L_ax_cs,M_ax_cs,N_ax_cs,O_ax_cs,P_ax_cs,[Q_ax_cs]]
all_ls_cs = [[A_ls_cs],B_ls_cs,C_ls_cs,D_ls_cs,E_ls_cs,F_ls_cs,G_ls_cs,H_ls_cs,I_ls_cs,J_ls_cs,K_ls_cs,L_ls_cs,M_ls_cs,N_ls_cs,O_ls_cs,P_ls_cs,[Q_ls_cs]]
for i in np.arange(0,len(all_ax_cs)):
    plt.scatter(all_ax_cs[i],all_ls_cs[i])
plt.scatter(all_ax_cs[0],all_ls_cs[0],s=100)
plt.xlim([0,350000])
plt.ylim([-5000,345000])
plt.title('entered: {} \n check E2 center is [ls,ax]: \n E12 center is {} \n  L10 center is {}'.format((E_ls_cs[1],E_ax_cs[1]),(E_ls_cs[11],E_ax_cs[11]),(L_ls_cs[9],L_ax_cs[9])))
#plt.scatter(current_file.ax_new_guess,current_file.ls_guess,c='k',alpha=0.2)
plt.savefig('../../../checkfig.png')


############################
# make csv
alpha = 'ABCDEFGHIJKLMNOPQ'
numera = np.arange(1,14).astype(str)

names = []
ls_guesses = []
ax_new_guesses = []

for i in np.arange(0,len(all_ax_cs)):
    curr_ax = all_ax_cs[i]
    curr_ls = all_ls_cs[i]

    for j in np.arange(0,len(curr_ax)):
        names.append(alpha[i]+numera[j])
        ls_guesses.append(int(curr_ls[j]))
        ax_new_guesses.append(int(curr_ax[j]))
new_dict = {'name':names,'ls_guess':ls_guesses,'ax_new_guess':ax_new_guesses}
new_csv = pd.DataFrame(new_dict)
now = datetime.now()
filestring = "../../../data/zaber_centers_"+now.strftime("%Y%m%d")+".csv"
filestring2 = "../../../data/zaber_centers.csv"
new_csv.to_csv(filestring,index=False)
try:
    new_csv.to_csv(filestring2,index=False)
except:
    print(' do you have zaber_centers.csv open? if so, close and try again')
##################################
# make ls map
# make empty array
new_ls = np.zeros((3500,3500,3))

# make top white (255), bottom is already black
top = int(E_ls_cs[0]/100) #divide by 100 bc zaber units are 350,000 not 3500
new_ls[0:top,0:3500]=255


# make horizontal 128 gray stripe
top  = int((A1c[0]+58900)/100)
bottom = int((A1c[0]+237900)/100)
new_ls[top:bottom,0:3500]=128


ls_low = int(A1_top_ls/100)
ls_high = int((A1c[0]+73700)/100)
ax_low = int((A1c[1]-129900)/100)
ax_high = int((A1c[1]-700)/100)
print(ls_low,ls_high,ax_low,ax_high)

[lss,axs] = utils.getpoints((ls_low,ax_high),(ls_high,ax_low)) # returns integer locations on line
# fill from val to high_ls, ax-1 to ax
for i in np.arange(1,len(lss)):
    if axs[i]<3500:
        new_ls[lss[i]:ls_high,axs[i]:ax_high]=128

# top right triangle fill

ls_low = int((A1c[0]-10900)/100)
if ls_low < 0:
    ls_low=1
ls_high = int((A1c[0]+58900)/100)
ax_high = int((A1c[1]+142300)/100)
ax_low = int((A1c[1]-700)/100)
print(ls_low,ls_high,ax_low,ax_high)

[lss,axs] = utils.getpoints((ls_low,ax_low),(ls_high,ax_high)) # returns integer locations on line

for i in np.arange(1,len(lss)):
    if axs[i]<3500:
        new_ls[lss[i]:ls_high,axs[i-1]:axs[i]]=128


# fill bottom left triangle
ls_high = int((A1c[0]+236900)/100)
ls_low = int((A1c[0]+294800)/100)
ax_high = int((A1c[1]-4100)/100)
ax_low = int((A1c[1]-121700)/100)
print(ls_low,ax_high,ls_high,ax_low)

[lss,axs] = utils.getpoints((ls_high,ax_low),(ls_low,ax_high+2)) # returns integer locations on line
for i in np.arange(1,len(lss)):
    new_ls[bottom:lss[i],axs[i-1]:axs[i]]=128

# fill bottom right triangle
ls_low = int((A1c[0]+216900)/100)
ls_high = int((A1c[0]+295400)/100) #increased empirically from 294800
ax_low = int((A1c[1]+36500)/100)
ax_high = int((A1c[1]+151700)/100)
print(ls_low,ax_high,ls_high,ax_low)
[lss,axs] = utils.getpoints((ls_low,ax_high),(ls_high,ax_low-1)) # returns integer locations on line
for i in np.arange(1,len(lss)):
    new_ls[bottom:lss[i],axs[i-1]:axs[i]]=128

# fill between triangles
ax_low = int((A1c[1]-4100)/100)

maxv = int((A1c[1]+436500)/100)
print(maxv)
new_ls[bottom:maxv,int((A1c[1]-4100)/100):int((A1c[1]+36500)/100)]=128

ls_tif_filestring='/Users/emily/Desktop/data/ls_limits.png'
ls_tif_filestring2='/Users/emily/Desktop/data/ls_limits_{}.png'.format(now.strftime("%Y%m%d"))
tif.imsave(ls_tif_filestring,new_ls.astype('uint8'),photometric='rgb')
tif.imsave(ls_tif_filestring2,new_ls.astype('uint8'),photometric='rgb')

########################
# make ax3

# make empty array
new_ax = np.zeros((3500,3500,3))

# first fill everything right of the middle with white
# B4 P4 get lowest ax value
[ls_high,ls_low,ax_high,ax_low]=utils.get_relative_vals(new_csv,"B4","P4",0)
new_ax[:,ax_low:3500]=255
plt.imshow(new_ax.astype('uint8'))

# next fill a vertical stripe
[ls_high,ls_low,ax_high,ax_low]=utils.get_relative_vals(new_csv,"B1","B4",0)
new_ax[0:3500,ax_low:ax_high]=128
b_left = ax_low
b_right = ax_high


# get top left triangle filled as 128
[a,b,c,ax_b1]=utils.get_relative_vals(new_csv,"B1","B4",0)
# want intersect of A1_top_ls/100 and ax_b1 - 60 as the top
ls_low = int(A1_top_ls/100) #int((A1c[0]-10900)/100)
ls_high = int((A1_top_ls+88580)/100) #a
ax_low =  int((A1_top_ax-130950)/100) #int((A1c[1]-129900)/100) #c
ax_high = int(A1_top_ax/100) #int((A1c[1]-700)/100)
print(ls_low,ls_high,ax_low,ax_high)
[lss,axs] = utils.getpoints((ls_high,ax_low),(ls_low,ax_high)) # returns integer locations on line
# find intersect and use this
for val in zip(lss,axs):
    if val[1]==ax_b1:
        ls_new=val[0]
#-60 below is from empirical observation 2025.03.29 by ejd
[lss,axs] = utils.getpoints((ls_high,ax_low),(ls_new-60,ax_b1+1)) # returns integer locations on line
# fill from val to high_ls, ax-1 to ax
for i in np.arange(1,len(lss)):
    if axs[i]<3500:
        new_ax[lss[i]:ls_high,axs[i-1]:axs[i]]=128


# top right triangle fill
#same ls_low
ls_high = int((A1_top_ls+68400)/100)
ax_low = int((A1_top_ax+141670)/100) #int((A1c[1]+142300)/100)
ax_high = int((A1_top_ax)/100)#d #int((A1c[1]-700)/100)
print(ls_low,ls_high,ax_low,ax_high)

[lss,axs] = utils.getpoints((ls_high,ax_low),(ls_low,ax_high+1)) # returns integer locations on line

for i in np.arange(1,len(lss)):
    if axs[i]<3500:
        new_ax[lss[i]:ls_high,axs[i-1]:axs[i]]=128

# fill bottom left triangle
ls_low = int((A1_top_ls+246030)/100) #was int((A1_top_ls+245040)/100) 
ls_high = int((A1_top_ls+305760)/100) #int(Q1c[0]/100) #int((A1c[0]+294800)/100)
if ls_high > 3080:
    ls_high = 3080
ax_low = int((A1_top_ax-119612)/100) #was -118912
ax_high =int((A1_top_ax+20910)/100) #was 19610
#ax_high = int((A1_top_ax+35280)/100) #c #int((A1c[1]-121700)/100)

[lss,axs] = utils.getpoints((ls_low,ax_low),(ls_high,ax_high)) # returns integer locations on line
for i in np.arange(1,len(lss)):
    new_ax[ls_low:lss[i],axs[i-1]:axs[i]]=128
print(ls_low,ls_high,ax_low,ax_high)

# fill bottom right triangle
ls_low = int((A1c[0]+216900)/100)
ls_high = int(Q1c[0]/100) #int((A1c[0]+294800)/100)
ax_low = int(Q1c[1]/100)
#ls_high = #int((A1c[0]+294800)/100)
#ax_low = #int((A1c[1]+36500)/100)
#ax_high = int((A1c[1]+151700)/100)
ax_high = int((M_ax_cs[-1]+8400)/100)
if ax_high > 3080:
    ax_high = 3080

[lss,axs] = utils.getpoints((ls_low,ax_high),(ls_high,ax_low-1)) # returns integer locations on line
for i in np.arange(1,len(lss)):
    new_ax[ls_low:lss[i],axs[i-1]:axs[i]]=128

# make a box for left side to fill center, then add a triangle
l = int((A1_top_ax-118912)/100)
r = int((A1_top_ax)/100)
t = int((A1_top_ls+88480)/100)
b = int((A1_top_ls+246030)/100) 

# fill square
new_ax[t:b,l:r]=128

r = int((A1_top_ax+141670)/100)
if r > 3080:
    r=3080
l = int((A1_top_ax)/100)
t = int((A1_top_ls+68480)/100)-1
b = int((A1_top_ls+225520)/100)+2
# fill square
new_ax[t:b,l:r]=128

# fill between triangles
ls_low = int((A1_top_ls+245680)/100)
ls_high = int((A1_top_ls+88680)/100) 
ax_low = int((A1_top_ax-130203)/100)
ax_high = int((A1_top_ax-118912)/100)


[lss,axs] = utils.getpoints((ls_low,ax_high),(ls_high,ax_low)) # returns integer locations on line
for i in np.arange(1,len(lss)):
    new_ax[ls_high:lss[i],axs[i]:ax_high]=128

# fill between triangles right

## want top right of box as starting point ls_low, ax_low
ax_low = int((A1_top_ax+141670)/100) #topright corner of box, t
ls_low = int((A1_top_ls+68480)/100) #top right corner of box, r

# now want the top right corner of the bottom right triangle
ax_high = int((M_ax_cs[-1]+8400)/100)
if ax_high > 3080:
    ax_high = 3080
ls_high = int((A1c[0]+216900)/100)
print(ls_low,ls_high,ax_low,ax_high)

[lss,axs] = utils.getpoints((ls_low,ax_low),(ls_high,ax_high)) # returns integer locations on line
for i in np.arange(1,len(lss)):
    new_ax[lss[i]:ls_high,ax_low:axs[i]]=128



### save

ax_tif_filestring='/Users/emily/Desktop/data/ax_limits.png'
ax_tif_filestring2 = '/Users/emily/Desktop/data/ax_limits_{}.png'.format(now.strftime("%Y%m%d"))
try:
    os.remove(ax_tif_filestring)
    print('removed file {}'.format(ax_tif_filestring))
    os.remove(ax_tif_filestring2)
    print('removed file {}'.format(ax_tif_filestring2))
except:
    print('no previous files found to delete, this is fine')
tif.imsave(ax_tif_filestring,new_ax.astype('uint8'),photometric='rgb')
tif.imsave(ax_tif_filestring2,new_ax.astype('uint8'),photometric='rgb')
print('saved new files')

#
